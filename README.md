# ProductAPI – feature/ef-core-sqlserver

## 📌 Resumen de esta etapa
Preparación y configuración de la persistencia de datos. Se conecta el dominio con SQL Server mediante Entity Framework Core, manteniendo la implementación aislada en la capa de Infraestructura.

## 🧭 Evolución del Proyecto (Diario de Desarrollo)
- **Posición**: Etapa 3.
- **Qué se heredó**: Entidades de dominio ricas y pruebas unitarias de eature/product-domain.
- **Qué se agregó**: Documentación técnica y planificación de instalación de paquetes Microsoft.EntityFrameworkCore.SqlServer y el contexto de datos.
- **Decisiones técnicas**: Se estableció explícitamente que la configuración de acceso a datos (ApplicationDbContext) residirá únicamente en ProductAPI.Infrastructure para proteger la inmutabilidad de la capa de Dominio.

## ✨ Funcionalidades
- Por confirmar (La configuración de base de datos está en progreso).

## 🛠️ Tecnologías
| Tecnología | Versión | Para qué se usa |
|---|---|---|
| .NET | 8.0 | Framework base |
| EF Core | (Pendiente) | Object-Relational Mapper (ORM) |
| SQL Server | (Pendiente) | Motor de persistencia relacional |

## 🚀 Instalación y ejecución
*(Nota: Pasos temporales, la base de datos aún no se ha generado)*
1. Clonar repositorio y cambiar a rama: git checkout feature/ef-core-sqlserver.
2. Restaurar solución: dotnet restore.

## 📂 Estructura del proyecto
`	ext
├── ProductAPI.Infrastructure/
│   ├── (Futuro) ApplicationDbContext.cs
│   └── (Futuro) Migrations/
`

## ➡️ Siguiente etapa
Desarrollo de los comandos y consultas (CQRS) con MediatR.
