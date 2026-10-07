# Capa de Presentación (ProductAPI.Api)

Esta es la "puerta de entrada" a nuestra aplicación. Es el proyecto de ASP.NET Core que expone nuestros servicios al mundo exterior a través de HTTP.
Depende de Application e Infrastructure (solo para armar el rompecabezas en la inyección de dependencias).

## 🛡️ Responsabilidades
1. **Controladores (Controllers / Endpoints):** Reciben las peticiones HTTP de los clientes (GET, POST, PUT, DELETE) y se las pasan a la capa de Aplicación (usando MediatR). ¡Los controladores no deben tener lógica de negocio!
2. **Configuración Inicial:** El archivo Program.cs se encarga de inyectar todas las dependencias.
3. **Middlewares:** Código que intercepta las peticiones para el manejo global de errores (ProblemDetails), validación de tokens de seguridad (JWT) y roles.
4. **Contenedorización (Docker):** Alojar el `Dockerfile` para empaquetar y ejecutar la API en entornos de contenedores junto con los demás servicios de la solución.
