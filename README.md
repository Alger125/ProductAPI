# ProductAPI

API de gestión de productos desarrollada con .NET 8 siguiendo los principios de Clean Architecture y un dominio rico (Rich Domain Model) con reglas de negocio encapsuladas.

## Tabla de contenidos
1. [Estado actual del proyecto](#1-estado-actual-del-proyecto)
2. [Arquitectura](#2-arquitectura)
3. [Estructura de la solución](#3-estructura-de-la-solución)
4. [Cómo se construyó la solución paso a paso](#4-cómo-se-construyó-la-solución-paso-a-paso)
5. [Capa de Dominio en detalle](#5-capa-de-dominio-en-detalle)
6. [Capa API: arranque y Swagger](#6-capa-api-arranque-y-swagger)
7. [Pruebas unitarias](#7-pruebas-unitarias)
8. [Cómo ejecutar el proyecto](#8-cómo-ejecutar-el-proyecto)
9. [Flujo de ramas e historial](#9-flujo-de-ramas-e-historial)
10. [Próximos pasos](#10-próximos-pasos)

---

## 1. Estado actual del proyecto

| Componente | Estado |
| --- | --- |
| Solución con 4 capas (Api, Application, Domain, Infrastructure) | Hecho |
| Entidades Product, Category, Brand, Review | Hecho |
| Reglas de negocio y encapsulación en Product | Hecho |
| Pruebas unitarias del dominio | Hecho |
| Swagger UI | Hecho |
| Persistencia con EF Core + SQL Server en Docker | Ver rama `feature/ef-core-sqlserver` |
| Casos de uso, endpoints de negocio, validaciones, CQRS | Pendiente |

*`main` representa el cascarón de la arquitectura y las reglas puras de dominio. Todavía no se usan librerías externas de CQRS ni de validación (sin MediatR ni FluentValidation).*

---

## 2. Arquitectura

**Regla de dependencias**
Las dependencias siempre apuntan hacia el dominio. El dominio no conoce a nadie.

```text
┌────────────────────────────────────────────────┐
│ ProductAPI.Api (presentación / host)           │
│       │                                │       │
│       ▼                                ▼       │
│  Application ◄── Infrastructure                │
│       │                                        │
│       ▼                                        │
│  Domain (entidades y reglas de negocio)        │
└────────────────────────────────────────────────┘
```

**Referencias entre proyectos (verificadas en los .csproj)**

| Proyecto | Referencia a | Paquetes NuGet |
| --- | --- | --- |
| `ProductAPI.Domain` | Ninguna | Ninguno |
| `ProductAPI.Application` | `Domain` | Ninguno |
| `ProductAPI.Infrastructure` | `Application` (y por transitividad `Domain`) | Ninguno en main |
| `ProductAPI.Api` | `Application`, `Infrastructure` | `Microsoft.AspNetCore.OpenApi 8.0.31`, `Swashbuckle.AspNetCore 6.6.2` |
| `ProductAPI.Domain.Tests` | `Domain` | Framework de pruebas |

*Por qué `Api` referencia a `Infrastructure`: el host necesita registrar las implementaciones concretas (repositorios, DbContext) en el contenedor de inyección de dependencias. Fuera de ese registro, la capa Api debe depender solo de abstracciones de Application.*

---

## 3. Estructura de la solución

```text
ProductAPI/
├── ProductAPI.sln
├── README.md
├── ProductAPI.Api/
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   ├── Program.cs
│   └── ProductAPI.Api.csproj
├── ProductAPI.Application/
│   └── ProductAPI.Application.csproj
├── ProductAPI.Domain/
│   ├── ProductAPI.Domain.csproj
│   └── Entities/
│       ├── Brand.cs
│       ├── Category.cs
│       ├── Product.cs
│       └── Review.cs
├── ProductAPI.Domain.Tests/
│   ├── ProductAPI.Domain.Tests.csproj
│   └── ProductTests.cs
└── ProductAPI.Infrastructure/
    └── ProductAPI.Infrastructure.csproj
```

---

## 4. Cómo se construyó la solución paso a paso
La solución se creó con la CLI de .NET. La secuencia equivalente es la siguiente.

### 4.1 Crear la solución y los proyectos
```bash
mkdir ProductAPI && cd ProductAPI
dotnet new sln -n ProductAPI
dotnet new webapi -n ProductAPI.Api -f net8.0
dotnet new classlib -n ProductAPI.Application -f net8.0
dotnet new classlib -n ProductAPI.Domain -f net8.0
dotnet new classlib -n ProductAPI.Infrastructure -f net8.0
```

### 4.2 Agregar los proyectos a la solución
```bash
dotnet sln add ProductAPI.Api/ProductAPI.Api.csproj
dotnet sln add ProductAPI.Application/ProductAPI.Application.csproj
dotnet sln add ProductAPI.Domain/ProductAPI.Domain.csproj
dotnet sln add ProductAPI.Infrastructure/ProductAPI.Infrastructure.csproj
```

### 4.3 Configurar las referencias entre capas
```bash
dotnet add ProductAPI.Application reference ProductAPI.Domain
dotnet add ProductAPI.Infrastructure reference ProductAPI.Application
dotnet add ProductAPI.Api reference ProductAPI.Application
dotnet add ProductAPI.Api reference ProductAPI.Infrastructure
```
Cada comando agrega un `<ProjectReference>` al `.csproj` del primer proyecto. Si se intenta crear una referencia circular, el compilador la rechaza, lo que ayuda a proteger la regla de dependencias.

### 4.4 Paquetes de la capa Api
```bash
dotnet add ProductAPI.Api package Swashbuckle.AspNetCore --version 6.6.2
dotnet add ProductAPI.Api package Microsoft.AspNetCore.OpenApi --version 8.0.31
```

### 4.5 Proyecto de pruebas del dominio
```bash
dotnet new xunit -n ProductAPI.Domain.Tests -f net8.0
dotnet sln add ProductAPI.Domain.Tests/ProductAPI.Domain.Tests.csproj
dotnet add ProductAPI.Domain.Tests reference ProductAPI.Domain
```

### 4.6 Verificar la compilación
```bash
dotnet build
dotnet test
```

---

## 5. Capa de Dominio en detalle

### 5.1 Entidades
| Entidad | Rol |
| --- | --- |
| `Product` | Agregado principal. Contiene precio, stock y las reglas de negocio |
| `Category` | Clasificación del producto (relación 1:N con Product) |
| `Brand` | Marca del producto (relación 1:N con Product) |
| `Review` | Reseña asociada a un producto (relación 1:N con Product) |

*Relaciones de Product: guarda CategoryId y BrandId como claves foráneas, con las propiedades de navegación Category y Brand, y una colección Reviews.*

### 5.2 Encapsulación en Product
Todas las propiedades tienen `private set`: el estado solo puede cambiar mediante métodos que validan las reglas de negocio. Esto evita que código externo deje la entidad en un estado inválido.

```csharp
public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }

    public Guid CategoryId { get; private set; }
    public Guid BrandId { get; private set; }

    public Category? Category { get; private set; }
    public Brand? Brand { get; private set; }
    public ICollection<Review> Reviews { get; private set; } = new List<Review>();

    protected Product() { } // constructor para EF Core

    public Product(Guid id, string name, decimal price, int stock, Guid categoryId, Guid brandId)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
        BrandId = brandId;

        UpdatePrice(price); // reutiliza la validación
        AddStock(stock);
    }
    // ...
}
```
**Decisiones de diseño:**
* Constructor `protected` sin parámetros: EF Core lo necesita para materializar entidades, y al no ser público impide crear productos sin pasar por las reglas.
* El constructor público delega en `UpdatePrice` y `AddStock`: así las reglas de validación existen en un único lugar y no se duplican.

### 5.3 Reglas de negocio
| Método | Regla | Excepción |
| --- | --- | --- |
| `UpdatePrice(decimal)` | El precio no puede ser negativo | ArgumentException |
| `AddStock(int)` | La cantidad a agregar no puede ser negativa | ArgumentException |
| `RemoveStock(int)` | La cantidad debe ser mayor a cero | ArgumentException |
| `RemoveStock(int)` | No se puede retirar más stock del disponible | InvalidOperationException |

Criterio de excepciones: `ArgumentException` indica un argumento inválido por sí mismo (negativo). `InvalidOperationException` indica que la operación no es válida dado el estado actual del objeto (stock insuficiente).

---

## 6. Capa API: arranque y Swagger

`Program.cs` en `main` es un host mínimo con Swagger:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Endpoint de ejemplo de la plantilla (se eliminará al crear los endpoints reales)
app.MapGet("/weatherforecast", () => { /* ... */ })
   .WithName("GetWeatherForecast")
   .WithOpenApi();

app.Run();
```

*Problema conocido con Swashbuckle 6.6.2: Swagger UI puede mostrar el error "La definición proporcionada no especifica un campo de versión válido" porque el documento se genera como openapi: 3.0.4. La solución aplicada en la rama `feature/ef-core-sqlserver` está documentada en su README.*

---

## 7. Pruebas unitarias
`ProductAPI.Domain.Tests/ProductTests.cs` verifica las reglas de negocio de `Product` sin depender de base de datos ni de la API. Las pruebas cubren los casos de las reglas de la sección 5.3 (precio negativo, stock negativo, retiro inválido y stock insuficiente).

```bash
dotnet test
```

---

## 8. Cómo ejecutar el proyecto
```bash
git clone https://github.com/Alger125/ProductAPI.git
cd ProductAPI
dotnet restore
dotnet build
dotnet run --project ProductAPI.Api
```
Abrir `https://localhost:<puerto>/swagger`. El puerto exacto se muestra en la consola al iniciar y se define en `Properties/launchSettings.json`.

---

## 9. Flujo de ramas e historial
Se trabaja con ramas de funcionalidad (`feature/*`) que se integran en `main` mediante Pull Requests. La convención de mensajes sigue Conventional Commits (`feat`, `build`, `docs`, `chore`).

| Rama | Documentación |
| --- | --- |
| `feature/product-domain` | Entidades y reglas de negocio |
| `feature/ef-core-sqlserver` | Docker, SQL Server y migraciones de EF Core |

---

## 10. Próximos pasos
* Definir interfaces de repositorio y casos de uso en `Application`.
* Implementar el `DbContext` y los repositorios en `Infrastructure`.
* Reemplazar el endpoint de ejemplo `/weatherforecast` por los endpoints de productos.
* Agregar validación de entrada (por ejemplo con FluentValidation) y manejo global de errores.
* Evaluar CQRS en caso de que la complejidad de los casos de uso lo justifyque.
