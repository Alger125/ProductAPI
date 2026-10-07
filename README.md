# ProductAPI – main

## 📌 Resumen de esta etapa
Inicialización del proyecto base de ProductAPI. Se definió la estructura inicial utilizando Clean Architecture.

## 🧭 Evolución del Proyecto (Diario de Desarrollo)
- **Posición**: Etapa 1.
- **Qué se heredó**: N/A.
- **Qué se agregó**: Solución ProductAPI.sln, Domain, Application, Infrastructure, Api.

## ✨ Funcionalidades
- Plantilla base (Sin endpoints del dominio).

## 🛠️ Tecnologías
| Tecnología | Versión | Para qué se usa |
|---|---|---|
| .NET | 8.0 | Framework base |
| C# | 12.0 | Lenguaje |

## 🚀 Instalación y ejecución
1. Ejecutar dotnet restore.
2. Ejecutar dotnet run --project ProductAPI.Api.

## 📂 Estructura del proyecto
`	ext
├── ProductAPI.Api/
├── ProductAPI.Application/
├── ProductAPI.Domain/
└── ProductAPI.Infrastructure/
`

## ➡️ Siguiente etapa
eature/product-domain.

---

## Evolución Global del Proyecto
El proyecto comenzó con la arquitectura en capas (main). Luego el núcleo de negocio encapsulado (eature/product-domain). Actualmente integrando persistencia (eature/ef-core-sqlserver).

| Etapa | Rama | Qué se logró | Link a la rama |
|---|---|---|---|
| 1 | main | Estructura base Clean Architecture. | [main](https://github.com/Alger125/ProductAPI/tree/main) |
| 2 | eature/product-domain | Entidades y reglas encapsuladas con xUnit. | [feature/product-domain](https://github.com/Alger125/ProductAPI/tree/feature/product-domain) |
| 3 | eature/ef-core-sqlserver | (En progreso) EF Core y SQL Server. | [feature/ef-core-sqlserver](https://github.com/Alger125/ProductAPI/tree/feature/ef-core-sqlserver) |
