# Capa de Aplicación (ProductAPI.Application)

Esta capa contiene los **Casos de Uso** (la lógica de la aplicación). 
Es la encargada de orquestar cómo se ejecutan las acciones en el sistema. **Solo depende de la capa de Dominio**.

## 🛡️ Responsabilidades
1. **CQRS (Commands & Queries):** Aquí usaremos MediatR para separar las operaciones que escriben/modifican datos (Commands) de las que solo leen datos (Queries).
2. **DTOs (Data Transfer Objects):** Clases simples que definen la información exacta que entra y sale de nuestra aplicación.
3. **Validación:** Usaremos FluentValidation para asegurar que los datos que envía el usuario sean correctos antes de intentar procesarlos.
4. **Interfaces de Servicios Externos:** Contratos para cosas como correos o notificaciones (la implementación real irá en Infraestructura).
