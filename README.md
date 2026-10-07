# Product API

API desarrollada con **.NET 8**, siguiendo principios de **Clean Architecture**.

## Arquitectura

El proyecto está dividido en las siguientes capas:

- ProductAPI.Api
- ProductAPI.Application
- ProductAPI.Domain
- ProductAPI.Infrastructure
- ProductAPI.Domain.Tests

## Progreso del proyecto

- [x] Estructura base de la solución.
- [x] Entidades del dominio.
- [x] Reglas de negocio de Product.
- [x] Pruebas unitarias del dominio.
- [ ] Entity Framework Core.
- [ ] SQL Server.
- [ ] ApplicationDbContext.
- [ ] Migraciones.
- [ ] CQRS con MediatR.
- [ ] Endpoints CRUD.
- [ ] FluentValidation.
- [ ] Manejo global de errores.
- [ ] Autenticación JWT.
- [ ] Docker.
- [ ] CI/CD.

## Ramas desarrolladas

### eature/product-domain

Estado: **Completada**

Implementa el núcleo del dominio de la aplicación:

- Product
- Category
- Brand
- Review
- Reglas de negocio
- Encapsulación
- Pruebas unitarias con xUnit

### eature/ef-core-sqlserver

Estado: **En progreso**

Su objetivo es conectar el dominio con SQL Server mediante Entity Framework Core.

Pendiente:

- Instalar Entity Framework Core.
- Configurar SQL Server.
- Crear ApplicationDbContext.
- Configurar entidades.
- Crear migraciones.
- Generar la base de datos.
