# ProductAPI – main

## 📌 Resumen de esta etapa
Inicialización del proyecto base de ProductAPI. Se definió la estructura inicial de la solución utilizando los principios de Clean Architecture.

## 🧭 Evolución del Proyecto (Diario de Desarrollo)
- **Posición**: Etapa 1.
- **Qué se heredó**: N/A (Inicio del repositorio).
- **Qué se agregó**: Creación de la solución ProductAPI.sln y los proyectos Domain, Application, Infrastructure y Api. Se configuraron las referencias para asegurar la inyección de dependencias hacia adentro.
- **Decisiones técnicas**: Se eligió Clean Architecture para desacoplar las reglas de negocio de la persistencia de datos y de la API web.

## ✨ Funcionalidades
- Proyecto base Web API de ASP.NET Core (plantilla por defecto). Aún sin endpoints del dominio.

## 🛠️ Tecnologías
| Tecnología | Versión | Para qué se usa |
|---|---|---|
| .NET | 8.0 | Framework base del proyecto |
| C# | 12.0 | Lenguaje de programación |

## 🚀 Instalación y ejecución
1. Clonar el repositorio.
2. Ejecutar dotnet restore.
3. Ejecutar dotnet run --project ProductAPI.Api.

## 📂 Estructura del proyecto
`	ext
ProductAPI.sln
├── ProductAPI.Api/
├── ProductAPI.Application/
├── ProductAPI.Domain/
└── ProductAPI.Infrastructure/
`

## ➡️ Siguiente etapa
Desarrollo del núcleo de negocio en la rama eature/product-domain.

---

## Evolución Global del Proyecto
El proyecto comenzó con la definición de la arquitectura en capas (main). Posteriormente, se construyó el núcleo de negocio completamente aislado y robusto con pruebas unitarias (eature/product-domain). Actualmente, la aplicación está siendo conectada a un motor SQL mediante el uso de Entity Framework Core (eature/ef-core-sqlserver).

| Etapa | Rama | Qué se logró | Link a la rama |
|---|---|---|---|
| 1 | main | Estructura base Clean Architecture. | [main](https://github.com/Alger125/ProductAPI/tree/main) |
| 2 | eature/product-domain | Entidades y reglas de negocio encapsuladas con xUnit tests. | [feature/product-domain](https://github.com/Alger125/ProductAPI/tree/feature/product-domain) |
| 3 | eature/ef-core-sqlserver | (En progreso) Configuración de EF Core y SQL Server en Infraestructura. | [feature/ef-core-sqlserver](https://github.com/Alger125/ProductAPI/tree/feature/ef-core-sqlserver) |
