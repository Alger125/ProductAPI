# Entity Framework Core + SQL Server

Rama:

`feature/ef-core-sqlserver`

## Objetivo

Agregar persistencia de datos a ProductAPI utilizando:

- Entity Framework Core
- SQL Server
- Docker (para la instancia de base de datos)
- LINQ (para consultas y operaciones sobre los datos)

La implementación debe mantenerse dentro de la arquitectura limpia del proyecto.

## Capa principal

La configuración de acceso a datos se realizará principalmente dentro de:

```text
ProductAPI.Infrastructure
```

La capa Domain debe permanecer independiente de Entity Framework Core y SQL Server.

## Implementación planeada

### Entity Framework Core

Agregar los paquetes necesarios:

```text
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Design
```

### ApplicationDbContext

Crear el contexto principal de la aplicación.

Deberá manejar las entidades:

```text
Products
Categories
Brands
Reviews
```

### Relaciones

Configurar las relaciones entre:

```text
Product
├── Category
├── Brand
└── Reviews
```

### Consultas con LINQ

- Consultas fuertemente tipadas sobre los `DbSet`.
- Carga de relaciones mediante `.Include()` y `.ThenInclude()`.
- Proyecciones eficientes (`.Select()`) y filtros (`.Where()`) traducidos a consultas SQL optimizadas.

### SQL Server y Docker

- Levantar la base de datos SQL Server mediante un contenedor Docker (usando imagen oficial de SQL Server o `docker-compose.yml`).
- Configurar la cadena de conexión correspondiente dentro de la configuración de la API (`appsettings.json`).

### Migraciones

Crear la primera migración para generar la estructura de la base de datos.

## Estado actual

**EN PROGRESO**

Actualmente la rama contiene la preparación y documentación de la arquitectura.

Todavía están pendientes:

- Dockerización de la base de datos SQL Server.
- Entity Framework Core.
- SQL Server.
- ApplicationDbContext.
- Configuraciones de entidades.
- Consultas y repositorios con LINQ.
- Migraciones.
- Creación de la base de datos.
