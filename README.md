# ProductAPI – Rama feature/domain-exceptions

En esta rama, hemos refinado la forma en que la API devuelve respuestas y maneja errores, alineándola a los estándares REST (201 Created) y creando excepciones propias para el modelo de negocio.

## Resumen de Cambios

### 1. Excepciones Personalizadas de Dominio
En Clean Architecture, el `Domain` no debe depender de excepciones del sistema general como `ArgumentException` o `InvalidOperationException` para representar reglas de negocio. En su lugar, es mejor tener nuestras propias excepciones semánticas.

Se crearon en `ProductAPI.Domain/Exceptions`:
* `DomainException`: Para cualquier regla de negocio rota (ej. "El precio no puede ser negativo").
* `NotFoundException`: Para cuando se busca un ID que no existe en la base de datos.

### 2. Refactorización en la clase Product
Modificamos nuestra entidad `Product` para que, en lugar de lanzar excepciones nativas genéricas, ahora dispare directamente nuestra nueva `DomainException` cada vez que se intenta crear un producto con un stock o precio inválido.

### 3. Mejora en el GlobalExceptionHandler
En la rama anterior habíamos construido nuestro "paraguas" global. Ahora, lo hemos configurado para entender nuestro lenguaje de negocio:
* Si atrapa una `DomainException` -> Devuelve un `400 Bad Request` automático.
* Si atrapa una `NotFoundException` -> Devuelve un `404 Not Found` automático (quedó listo para cuando se implementen los *Queries* por ID).

### 4. Respuestas `201 Created` en Controladores
Antes, cuando enviábamos un POST a `api/Products`, la API devolvía un estatus genérico `200 OK`. Según los estándares HTTP (REST), cuando una petición resulta en la creación exitosa de un recurso, el estatus correcto debe ser **201 Created**.
Pasamos por los 4 controladores (`Products`, `Brands`, `Categories` y `Reviews`) para asegurar que ahora todas sus peticiones POST devuelven correctamente el estatus 201.

## ¿Qué ganamos con esto?
* **Semántica REST impecable:** Las creaciones exitosas se marcan como `201` y los errores o cosas no encontradas como `400` y `404` respectivamente, facilitando la vida a los desarrolladores de frontend.
* **Separación de responsabilidades limpia:** La base del Dominio ya no depende de excepciones del sistema.
* **ProblemDetails Automático:** Las nuevas excepciones siguen aprovechando nuestro middleware para salir renderizadas como un objeto JSON `ProblemDetails` súper detallado y elegante.
