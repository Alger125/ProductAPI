# ProductAPI – Rama feature/product-full-crud

En esta rama, hemos completado el ciclo de vida (CRUD) para la entidad principal de nuestro sistema: **Product**. Ahora la API permite buscar por ID, actualizar datos y eliminar registros usando Clean Architecture y CQRS con MediatR.

## Resumen de Cambios

### 1. Nuevos Casos de Uso (Application Layer)
Se crearon los siguientes Comandos y Consultas con sus respectivos Handlers:
* `GetProductByIdQuery`: Consulta la base de datos por el GUID del producto. Si no lo encuentra, lanza nuestra `NotFoundException` (la cual es traducida automáticamente a un Error 404 por nuestro middleware global).
* `UpdateProductCommand`: Permite actualizar el precio y añadir stock al producto. Reutiliza las reglas de negocio del dominio (`UpdatePrice` y `AddStock`).
* `DeleteProductCommand`: Elimina físicamente el producto de la base de datos.

### 2. Refinamiento del Controlador (Api Layer)
El `ProductsController` se actualizó para exponer los nuevos endpoints RESTful:
* `GET /api/Products/{id}`
* `PUT /api/Products/{id}`
* `DELETE /api/Products/{id}`

### 3. El estándar Location en el POST
Ahora que tenemos un endpoint `GET /api/Products/{id}`, modificamos la acción de creación (`POST`). En lugar de usar un simple `StatusCode(201)`, ahora usamos `CreatedAtAction`. 
Esto asegura que, al crear un producto exitosamente, la API devuelva el `201 Created` **y** adjunte el encabezado HTTP `Location: /api/Products/{id}`, lo cual es el estándar más estricto de una API REST madura.

---

## Ejemplos de Endpoints

### 1. Obtener un Producto (GET)
**Ruta:** `GET /api/Products/3fa85f64-5717-4562-b3fc-2c963f66afa6`
**Respuesta Exitosa (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "Teclado Mecánico",
  "description": "Switch Red",
  "price": 120.50,
  "stock": 10,
  "categoryId": "...",
  "brandId": "..."
}
```
**Respuesta Fallida (404 Not Found):** (Gracias al GlobalExceptionHandler)
```json
{
  "title": "Recurso no encontrado",
  "status": 404,
  "detail": "La entidad \"Product\" con el ID (0000000-...) no fue encontrada."
}
```

### 2. Actualizar un Producto (PUT)
**Ruta:** `PUT /api/Products/3fa85f64-5717-4562-b3fc-2c963f66afa6`
**Cuerpo de la Petición:**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "price": 150.00,
  "stockToAdd": 5
}
```
**Respuesta Exitosa:** `204 No Content` (El estándar para actualizaciones donde no hay contenido extra que devolver).

### 3. Eliminar un Producto (DELETE)
**Ruta:** `DELETE /api/Products/3fa85f64-5717-4562-b3fc-2c963f66afa6`
**Respuesta Exitosa:** `204 No Content`.
**Respuesta Fallida (Si no existe):** `404 Not Found`.

### 4. Crear Producto (El nuevo POST)
**Ruta:** `POST /api/Products`
**Respuesta Exitosa:** `201 Created`
**Encabezados Incluidos:**
```text
Location: https://localhost:7112/api/Products/3fa85f64-5717-4562-b3fc-2c963f66afa6
```
*(El cliente frontend puede leer ese encabezado y saber exactamente dónde buscar el producto recién guardado).*
