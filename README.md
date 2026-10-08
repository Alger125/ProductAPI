# ProductAPI – Rama feature/error-handling-validation

Implementación de manejo global de excepciones y validación de entrada (Pipeline Behaviors) en una Arquitectura Limpia usando .NET 8.

## Tabla de contenidos
1. [Objetivo de la rama](#1-objetivo-de-la-rama)
2. [Punto de partida y el problema a resolver](#2-punto-de-partida-y-el-problema-a-resolver)
3. [Manejo Global de Excepciones (.NET 8)](#3-manejo-global-de-excepciones-net-8)
4. [Validación de Entrada con FluentValidation](#4-validación-de-entrada-con-fluentvalidation)
5. [Pipeline Behavior: El Interceptor de MediatR](#5-pipeline-behavior-el-interceptor-de-mediatr)
6. [Respuestas HTTP y ProblemDetails](#6-respuestas-http-y-problemdetails)
7. [Cómo probarlo en Swagger](#7-cómo-probarlo-en-swagger)
8. [Beneficios Arquitectónicos](#8-beneficios-arquitectónicos)

---

## 1. Objetivo de la rama
Mejorar la madurez y resiliencia de la API. Antes de esta rama, cualquier error de validación o del dominio rompía el flujo y retornaba un genérico **500 Internal Server Error**. 

En esta rama se implementaron:
* Un manejador global de excepciones (`IExceptionHandler`) para capturar errores sin usar bloques `try/catch` en los controladores.
* `FluentValidation` para validar los comandos (inputs) antes de procesarlos.
* Un `IPipelineBehavior` de MediatR para ejecutar automáticamente las validaciones.
* El estándar de respuestas `ProblemDetails` para informar al cliente exactamente qué salió mal (Status 400).

---

## 2. Punto de partida y el problema a resolver
Anteriormente, si enviabas un producto con un precio de `-10`, la capa de Dominio lanzaba una `ArgumentException`. Al no haber nadie que atrapara esa excepción, el servidor colapsaba la petición y le devolvía al cliente web un `500 Internal Server Error`. 

Esto es una mala práctica:
1. Un `500` significa "El servidor falló", cuando en realidad fue el cliente quien mandó datos inválidos (debió ser un `400 Bad Request`).
2. Poner `try/catch` en cada uno de los Controladores ensucia el código y viola el principio DRY (Don't Repeat Yourself).

---

## 3. Manejo Global de Excepciones (.NET 8)

A partir de .NET 8, la forma recomendada de manejar excepciones globales es implementando la interfaz `IExceptionHandler`.

**Archivo:** `ProductAPI.Api/Middlewares/GlobalExceptionHandler.cs`

Esta clase actúa como un "paraguas". Atrapa todas las excepciones de la aplicación y decide cómo responder:
```csharp
public async ValueTask<bool> TryHandleAsync(...)
{
    // 1. Logueamos el error para los desarrolladores
    _logger.LogError(exception, "Ha ocurrido un error no controlado.");

    // 2. Evaluamos de dónde viene el error
    if (exception is ArgumentException || exception is InvalidOperationException)
    {
        // Es un error de nuestras reglas de negocio del Dominio
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "Error de regla de negocio";
        problemDetails.Detail = exception.Message;
    }
    else if (exception is FluentValidation.ValidationException)
    {
        // Es un error de entrada de datos (JSON mal formado o inválido)
        problemDetails.Status = StatusCodes.Status400BadRequest;
        problemDetails.Title = "Error de validación";
        // ... se mapean los errores específicos
    }
    // 3. Devolvemos el JSON al usuario
}
```

---

## 4. Validación de Entrada con FluentValidation

El Dominio es el encargado de proteger sus propias reglas matemáticas (ej. Stock >= 0). Pero la capa de **Aplicación** es la encargada de validar que los comandos (`Commands`) vengan con la información obligatoria antes de intentar hacer nada.

Para esto instalamos `FluentValidation` y creamos validadores semánticos.

**Archivo:** `ProductAPI.Application/Features/Products/Commands/CreateProduct/CreateProductCommandValidator.cs`

```csharp
public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(100);

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");
            
        // ... más reglas
    }
}
```
*Ventaja:* Las validaciones son expresivas, fluídas y están completamente separadas del Modelo o del Controlador.

---

## 5. Pipeline Behavior: El Interceptor de MediatR

Tener un validador no sirve de nada si nadie lo ejecuta. En lugar de inyectar el validador en el Handler y llamar a `.Validate()`, usamos un **Pipeline Behavior** de MediatR.

Un "Behavior" es un middleware interno de la capa de Aplicación. Cada vez que alguien envía un `Command` (ej. Crear Producto), el Behavior lo intercepta en el aire:

**Archivo:** `ProductAPI.Application/Behaviors/ValidationBehavior.cs`

1. Toma el `Command`.
2. Busca si existe un Validador para ese `Command`.
3. Lo ejecuta.
4. Si hay errores (ej. El nombre está vacío), lanza una `ValidationException` cortando el flujo inmediatamente.
5. Si todo está bien, permite que el `Command` llegue a su destino final (el `Handler`).

---

## 6. Respuestas HTTP y ProblemDetails

Para estandarizar cómo se comunican los errores a aplicaciones frontend o clientes móviles, .NET utiliza el formato estándar RFC 7807 llamado **ProblemDetails**.

Al registrar `builder.Services.AddProblemDetails();` en `Program.cs`, nos aseguramos de que todos los errores regresen con esta estructura JSON predecible:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
  "title": "Error de validación de datos de entrada",
  "status": 400,
  "detail": "Uno o más campos tienen errores de validación.",
  "errors": [
    {
      "propertyName": "Name",
      "errorMessage": "El nombre del producto es obligatorio."
    }
  ]
}
```

---

## 7. Cómo probarlo en Swagger

1. Clona la rama y arranca la API (`dotnet run --project ProductAPI.Api`).
2. Abre Swagger (`http://localhost:<puerto>/swagger`).
3. Ve al endpoint `POST /api/Products`.
4. Intenta enviar este JSON inválido:
```json
{
  "name": "",
  "description": "Prueba",
  "price": -1500,
  "stock": -5,
  "categoryId": "00000000-0000-0000-0000-000000000000",
  "brandId": "00000000-0000-0000-0000-000000000000"
}
```
**Resultado esperado:** El servidor **no va a fallar**. Te responderá de inmediato con un código `400 Bad Request` listando todos y cada uno de los errores (Nombre vacío, Precio negativo, Stock negativo).

---

## 8. Beneficios Arquitectónicos

1. **Fail-Fast (Fallo Rápido):** Las peticiones inválidas son rechazadas en milisegundos sin consumir memoria instanciando el Dominio ni abriendo conexiones a Base de Datos.
2. **Controladores puros:** El `ProductsController` sigue teniendo solo 2 líneas de código. No tiene ningún `if(!ModelState.IsValid)`.
3. **Manejo Centralizado:** Si en el futuro queremos mandar los errores a DataDog, Sentry o Application Insights, solo tenemos que modificar 1 línea en el `GlobalExceptionHandler`. No hay que tocar los 50 endpoints de la API.
