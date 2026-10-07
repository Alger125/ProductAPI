# Entity Framework Core + SQL Server

Rama:

eature/ef-core-sqlserver

## Objetivo

Agregar persistencia de datos a ProductAPI utilizando:

- Entity Framework Core
- SQL Server

La implementación debe mantenerse dentro de la arquitectura limpia del proyecto.

## Capa principal

La configuración de acceso a datos se realizará principalmente dentro de:

`	ext
ProductAPI.Infrastructure
`

La capa Domain debe permanecer independiente de Entity Framework Core y SQL Server.

## Implementación planeada

### Entity Framework Core

Agregar los paquetes necesarios:

`	ext
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Design
`

### ApplicationDbContext

Crear el contexto principal de la aplicación.

Deberá manejar las entidades:

`	ext
Products
Categories
Brands
Reviews
`

### Relaciones

Configurar las relaciones entre:

`	ext
Product
├── Category
├── Brand
└── Reviews
`

### SQL Server

Agregar la cadena de conexión correspondiente dentro de la configuración de la API.

### Migraciones

Crear la primera migración para generar la estructura de la base de datos.

## Estado actual

**EN PROGRESO**

Actualmente la rama contiene la preparación y documentación de la arquitectura.

Todavía están pendientes:

- Entity Framework Core.
- SQL Server.
- ApplicationDbContext.
- Configuraciones de entidades.
- Migraciones.
- Creación de la base de datos.
