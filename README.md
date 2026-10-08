# ProductAPI – Rama feature/product-usecases

¡Hola, dev! 👋 Si estás leyendo esto, es porque vamos a dar el siguiente gran paso en nuestra Clean Architecture. Hasta ahora teníamos un "Cerebro" muy inteligente (la capa **Domain**) y una "Memoria" conectada a SQL Server (la capa **Infrastructure**). 

Pero nadie podía hablar con ellos. Estaban aislados.

El objetivo de esta rama es crear los **Casos de Uso (Application Layer)** y exponerlos al mundo a través de **Controladores HTTP (Api Layer)** usando el patrón **CQRS**. Te voy a explicar el paso a paso de lo que hicimos aquí, como si estuviéramos programando en pareja.

---

## Tabla de Contenidos
1. [¿Qué es el Patrón CQRS y MediatR?](#1-qué-es-el-patrón-cqrs-y-mediatr)
2. [El Patrón Repositorio (Interfaces vs Implementación)](#2-el-patrón-repositorio-interfaces-vs-implementación)
3. [DTOs y Mapeo (LINQ)](#3-dtos-y-mapeo-linq)
4. [Controladores sin lógica de negocio](#4-controladores-sin-lógica-de-negocio)
5. [Inyección de Dependencias](#5-inyección-de-dependencias)
6. [Cómo probar la API en Swagger](#6-cómo-probar-la-api-en-swagger)

---

## 1. ¿Qué es el Patrón CQRS y MediatR?
CQRS significa *Command and Query Responsibility Segregation* (Separación de Responsabilidades de Comandos y Consultas).
* **Command (Comando):** Es una orden para modificar el sistema (Ej: Crear, Actualizar, Borrar).
* **Query (Consulta):** Es una pregunta al sistema. Solo lee datos, NUNCA los modifica (Ej: Obtener Todos).

Para implementar esto en .NET, usamos una librería súper famosa llamada **MediatR**.
MediatR funciona como un cartero: el Controlador (API) le entrega una carta (un `Command` o `Query`), y MediatR sabe exactamente a qué manejador (`Handler`) entregársela para que haga el trabajo.

### Ejemplo de cómo lo programamos:
1. Creamos la "carta": `CreateProductCommand.cs` (Un objeto tipo `record` que solo transporta datos).
2. Creamos al "trabajador": `CreateProductCommandHandler.cs` (La clase que recibe la carta, crea la entidad del dominio, y la guarda en la base de datos).

---

## 2. El Patrón Repositorio (Interfaces vs Implementación)
La regla de oro de Clean Architecture es que el centro (Application) **nunca** debe depender de la periferia (Infrastructure).

Si pusiéramos `ApplicationDbContext` (de EF Core) directo en nuestros Handlers, estaríamos casando nuestra lógica de negocio con SQL Server. ¿La solución? El patrón Repositorio.

1. **En la capa de Application (El Contrato):**
   Creamos `IProductRepository`. Es solo una interfaz. Le dice al mundo: *"No me importa si eres SQL, MongoDB o un Excel, necesito que me des un método `AddAsync` y un `GetAllAsync`"*.
   
2. **En la capa de Infrastructure (El Trabajador):**
   Creamos `ProductRepository`. Esta clase sí instala Entity Framework Core, hereda de `IProductRepository`, y hace las consultas reales a la base de datos usando SQL.

---

## 3. DTOs y Mapeo (LINQ)
Nunca debes devolver tu Entidad pura del Dominio al cliente de la API (a Swagger o a un frontend). Las entidades pueden tener lógica interna o datos sensibles. En su lugar, devolvemos un **DTO (Data Transfer Object)**.

Creamos records como `ProductDto` que solo contienen las propiedades que queremos mostrar.

**¿Qué pasa con el "Linkeo/Mapeo" (LINQ)?**
En nuestro `GetProductsQueryHandler`, al obtener las entidades de la base de datos, usamos **LINQ** (Language Integrated Query) para transformar mágicamente las Entidades en DTOs usando `.Select()`:
```csharp
var productDtos = products.Select(p => new ProductDto(
    p.Id, p.Name, p.Description, p.Price, p.Stock, p.CategoryId, p.BrandId
));
```
*(Nota Senior: En este proyecto hicimos el "mapeo" de forma manual con LINQ para tener control total. En proyectos gigantescos, se suele usar una librería llamada `AutoMapper` que hace esto automáticamente, ¡pero LINQ manual rinde más rápido!)*

---

## 4. Controladores sin lógica de negocio
Con MediatR y nuestros Casos de Uso listos, la capa API queda ridículamente delgada (¡Y eso es excelente!).

Si miras `ProductsController.cs`, verás que el método solo tiene 2 líneas de código real:
```csharp
[HttpPost]
public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
{
    var productId = await _mediator.Send(command); // 1. Mandar a MediatR
    return Ok(new { Message = "Producto creado", ProductId = productId }); // 2. Responder 200 OK
}
```
**Regla Senior:** El controlador no valida reglas, no guarda en bases de datos y no sabe de SQL. Solo recibe HTTP y contesta HTTP.

---

## 5. Inyección de Dependencias
Para que todo este rompecabezas de interfaces y Handlers se conecte cuando arranca la aplicación, fuimos al archivo `Program.cs` de nuestra API y amarramos los cables:

```csharp
// Le decimos al sistema: "Cuando alguien pida un IProductRepository, entrégale un ProductRepository real"
builder.Services.AddScoped<IProductRepository, ProductRepository>();
// (Se hizo lo mismo para Brands, Categories y Reviews)

// Instalamos MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));
```

---

## 6. Cómo probar la API en Swagger

Hicimos este mismo flujo (Repository -> Command/Query -> DTO -> Controller) para nuestras **4 entidades**: `Products`, `Brands`, `Categories` y `Reviews`.

Para probarlo localmente:
1. Clona el proyecto y asegúrate de que tu contenedor de SQL Server en Docker esté corriendo.
2. Abre una terminal y corre:
   ```bash
   dotnet run --project ProductAPI.Api
   ```
3. Abre tu navegador en `http://localhost:5213/swagger`

**Importante:** Recuerda que las bases de datos relacionales son estrictas. 
Si intentas crear un **Producto**, te va a pedir un `CategoryId` y un `BrandId`. Si pones identificadores falsos, el servidor te regresará un *Error 500* por violación de Llave Foránea (Foreign Key Constraint). 
**Debes seguir este orden:**
1. Ve al endpoint de `Categories` y crea una categoría por `POST`. (Copia el ID que te devuelve).
2. Ve al endpoint de `Brands` y crea una marca por `POST`. (Copia el ID).
3. ¡Ahora sí! Usa esos dos IDs para crear tu `Product`.
