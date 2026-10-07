# ProductAPI – feature/ef-core-sqlserver

## 📌 Resumen de esta etapa
Preparación y configuración de la persistencia de datos. Se conecta el dominio con SQL Server mediante Entity Framework Core, manteniendo la implementación aislada en la capa de Infraestructura, planificando la contenedorización con Docker y el uso de LINQ para consultas optimizadas.

## 🧭 Evolución del Proyecto (Diario de Desarrollo)
- **Posición**: Etapa 3.
- **Qué se heredó**: Entidades de dominio ricas y pruebas unitarias de `feature/product-domain`.
- **Qué se agregó**: Documentación técnica, planificación de instalación de paquetes Microsoft.EntityFrameworkCore.SqlServer, contexto de datos, contenedorización con Docker y diseño de consultas con LINQ.
- **Decisiones técnicas**: 
  - Se estableció explícitamente que la configuración de acceso a datos (ApplicationDbContext) residirá únicamente en `ProductAPI.Infrastructure` para proteger la inmutabilidad de la capa de Dominio.
  - Se utilizará Docker para desplegar y estandarizar la instancia de SQL Server (y posteriormente la API) mediante contenedores, asegurando un entorno de desarrollo consistente.
  - Se implementará **LINQ (Language Integrated Query)** para estructurar consultas fuertemente tipadas, proyecciones eficientes a DTOs (`Select`), filtros, ordenamientos y paginación sobre `IQueryable`.

## ✨ Funcionalidades
- Por confirmar (La configuración de base de datos, LINQ y contenedorización están en progreso).

## 🛠️ Tecnologías
| Tecnología | Versión | Para qué se usa |
|---|---|---|
| .NET | 8.0 | Framework base |
| EF Core | (Pendiente) | Object-Relational Mapper (ORM) |
| SQL Server | (Pendiente) | Motor de persistencia relacional |
| Docker | (Pendiente) | Contenedorización de SQL Server y la API |
| LINQ | .NET 8 | Consultas fuertemente tipadas y optimizadas hacia la base de datos |

## 🚀 Instalación y ejecución
*(Nota: Pasos temporales, la base de datos y contenedores aún no se han generado)*
1. Clonar repositorio y cambiar a rama: `git checkout feature/ef-core-sqlserver`.
2. Levantar servicios en Docker (próximamente): `docker compose up -d`.
3. Restaurar solución: `dotnet restore`.

## 📁 Estructura del proyecto
```text
├── docker-compose.yml (Futuro)
├── ProductAPI.Infrastructure/
│   ├── (Futuro) ApplicationDbContext.cs
│   └── (Futuro) Migrations/
```

## ➡️ Siguiente etapa
Desarrollo de los comandos y consultas (CQRS) con MediatR y LINQ.
