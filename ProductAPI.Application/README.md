# Capa de Aplicación (ProductAPI.Application)

Esta capa contiene los **Casos de Uso** (la lógica de la aplicación). 
Es la encargada de orquestar cómo se ejecutan las acciones en el sistema. **Solo depende de la capa de Dominio**.

## 🛡️ Responsabilidades
1. **CQRS (Commands & Queries):** Aquí usaremos MediatR para separar las operaciones que escriben/modifican datos (Commands) de las que solo leen datos (Queries).
2. **Consultas con LINQ (Language Integrated Query):** En los manejadores de consultas (Queries), se utilizará LINQ para filtrado dinámico, ordenamiento, paginación y proyecciones directas a DTOs para máximo rendimiento.
3. **DTOs (Data Transfer Objects):** Clases simples que definen la información exacta que entra y sale de nuestra aplicación.
4. **Validación:** Usaremos FluentValidation para asegurar que los datos que envía el usuario sean correctos antes de intentar procesarlos.
5. **Interfaces de Servicios Externos:** Contratos para cosas como correos o notificaciones (la implementación real irá en Infraestructura).
