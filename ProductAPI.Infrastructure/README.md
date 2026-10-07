# Capa de Infraestructura (ProductAPI.Infrastructure)

Esta capa contiene los **detalles técnicos** y la comunicación con el mundo exterior. 
Depende de la capa de Aplicación para poder implementar los contratos/interfaces que esta defina.

## 🛡️ Responsabilidades
1. **Acceso a Datos (Base de Datos):** Aquí vivirá **Entity Framework Core**, el ApplicationDbContext y las migraciones para SQL Server.
2. **Implementación de Repositorios:** Clases concretas que van a la base de datos a buscar, crear o modificar los datos de nuestro Dominio.
3. **Servicios Externos:** Conexiones a APIs de terceros, sistemas de archivos, cachés (Redis) o envíos de correos.
