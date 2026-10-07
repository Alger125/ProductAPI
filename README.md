# ProductAPI – feature/product-domain

## 📌 Resumen de esta etapa
Creación del núcleo de negocio (Dominio) de ProductAPI, manteniendo la capa completamente independiente de bases de datos, APIs y otros componentes externos.

## 🧭 Evolución del Proyecto (Diario de Desarrollo)
- **Posición**: Etapa 2 de la evolución actual.
- **Qué se heredó**: Estructura de proyectos Clean Architecture de la rama main.
- **Qué se agregó**: Entidades (Product, Category, Brand, Review), reglas de negocio encapsuladas y el proyecto de pruebas unitarias (ProductAPI.Domain.Tests).
- **Decisiones técnicas**: Se aplicó el patrón de *Modelo de Dominio Rico*, usando private set en las propiedades para evitar modificaciones anémicas y garantizar que los cambios de estado pasen por métodos validadores (ej. UpdatePrice).
- **Problemas resueltos**: Se protegió la integridad del producto garantizando que precios y stocks no puedan ser negativos (excepciones ArgumentException interceptadas en pruebas).

## ✨ Funcionalidades
- Por confirmar (Aún no hay endpoints HTTP expuestos, la lógica es puramente interna).

## 🛠️ Tecnologías
| Tecnología | Versión | Para qué se usa |
|---|---|---|
| .NET | 8.0 | Framework base |
| xUnit | (latest) | Framework para pruebas unitarias de dominio |

## 🚀 Instalación y ejecución
1. Clonar el repositorio y cambiar a la rama: git checkout feature/product-domain.
2. Restaurar paquetes: dotnet restore.
3. Ejecutar pruebas unitarias para verificar el dominio: dotnet test ProductAPI.Domain.Tests.

## 📂 Estructura del proyecto
`	ext
├── ProductAPI.Domain/
│   └── Entities/
│       ├── Brand.cs
│       ├── Category.cs
│       ├── Product.cs
│       └── Review.cs
└── ProductAPI.Domain.Tests/
    └── ProductTests.cs
`

## ➡️ Siguiente etapa
Integración de la base de datos (Persistencia) y configuración del ORM en la rama eature/ef-core-sqlserver.
