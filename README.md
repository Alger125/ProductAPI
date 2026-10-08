# ProductAPI – Rama feature/product-domain

Implementación del núcleo del negocio (Core Domain) de ProductAPI con un Rich Domain Model, siguiendo Clean Architecture en .NET 8.

## Tabla de contenidos
1. [Objetivo de la rama](#1-objetivo-de-la-rama)
2. [Alcance: qué incluye y qué no](#2-alcance-qué-incluye-y-qué-no)
3. [Arquitectura y estructura](#3-arquitectura-y-estructura)
4. [Desarrollo paso a paso](#4-desarrollo-paso-a-paso)
5. [Diseño del dominio: por qué un Rich Domain Model](#5-diseño-del-dominio-por-qué-un-rich-domain-model)
6. [Entidades y relaciones](#6-entidades-y-relaciones)
7. [Reglas de negocio encapsuladas](#7-reglas-de-negocio-encapsuladas)
8. [Ejemplos de uso](#8-ejemplos-de-uso)
9. [Decisiones de diseño y limitaciones conocidas](#9-decisiones-de-diseño-y-limitaciones-conocidas)
10. [Verificación](#10-verificación)
11. [Próximos pasos](#11-próximos-pasos)

---

## 1. Objetivo de la rama
Diseñar el corazón de la aplicación: la capa de Dominio. Se definen las entidades `Product`, `Brand`, `Category` y `Review` de forma pura, sin depender de base de datos, frameworks externos ni capa de presentación.

En lugar de un dominio anémico (clases que son solo bolsas de datos con getters y setters), se implementó un Rich Domain Model: las reglas de negocio viven y se protegen dentro de las propias entidades.

---

## 2. Alcance: qué incluye y qué no

| Incluye | No incluye (otras ramas) |
| --- | --- |
| Entidades `Product`, `Category`, `Brand`, `Review` | Persistencia con EF Core y SQL Server → `feature/ef-core-sqlserver` |
| Reglas de negocio en `Product` (precio y stock) | Pruebas unitarias del dominio → `main` (`ProductAPI.Domain.Tests`) |
| Encapsulación con `private set` | Casos de uso, endpoints y validación de entrada |
| Constructor defensivo | |

---

## 3. Arquitectura y estructura
```text
ProductAPI.Domain/
├── ProductAPI.Domain.csproj
└── Entities/
    ├── Brand.cs
    ├── Category.cs
    ├── Product.cs
    └── Review.cs
```

**Regla de dependencias:** `ProductAPI.Domain` no referencia ningún otro proyecto ni paquete NuGet (ni `Microsoft.EntityFrameworkCore` ni otros). Es C# puro. Todas las demás capas dependen del dominio; el dominio no depende de nadie.

```text
Api ──► Application ──► Domain
Api ──► Infrastructure ──► Application
(el dominio no apunta a ninguna capa)
```

---

## 4. Desarrollo paso a paso

### 4.1 Crear la rama de trabajo
```bash
git checkout main
git pull origin main
git checkout -b feature/product-domain
```

### 4.2 Crear la estructura de carpetas del dominio
```bash
mkdir ProductAPI.Domain/Entities
```
Si la plantilla de classlib generó un `Class1.cs`, se elimina:
```bash
rm ProductAPI.Domain/Class1.cs
# Windows PowerShell: Remove-Item ProductAPI.Domain\Class1.cs
```

### 4.3 Crear las entidades
Se crean los cuatro archivos en `ProductAPI.Domain/Entities/` con el namespace `ProductAPI.Domain.Entities`:
`Brand.cs`, `Category.cs`, `Product.cs`, `Review.cs`

Primer commit de la rama:
```bash
dotnet build
git add .
git commit -m "feat: add Product, Category, Brand and Review entities"
```

### 4.4 Agregar encapsulación y reglas de negocio a Product
Se cambian los setters públicos por `private set`, se agrega el constructor público defensivo, el constructor `protected` para ORMs y los métodos `UpdatePrice`, `AddStock` y `RemoveStock` (detalle en la sección 7).
```bash
dotnet build
git add .
git commit -m "feat: add business rules and encapsulation to Product entity"
```

### 4.5 Documentar y abrir el Pull Request
```bash
git add .
git commit -m "docs: update root README with PRD progress and add Domain documentation"
git push -u origin feature/product-domain
```
Luego se abre un Pull Request hacia `main` en GitHub. Esta rama se integró como PR #1.
*Los mensajes de commit siguen Conventional Commits (`feat`, `docs`, `build`, `chore`).*

---

## 5. Diseño del dominio: por qué un Rich Domain Model
En un proyecto tradicional, la validación de un producto (por ejemplo "el precio no puede ser negativo") suele escribirse en servicios o controladores. Eso obliga a repetirla en cada lugar que modifica el precio. Aquí esa responsabilidad se movió a la propia entidad.

| Enfoque | Dónde viven las reglas | Riesgo |
| --- | --- | --- |
| Dominio anémico | En servicios o controladores | Un código nuevo puede olvidar la validación y guardar datos inválidos |
| **Rich Domain Model** (esta rama) | Dentro de la entidad | Para cambiar el estado hay que pasar por un método que valida |

**Beneficios:**
* **Seguridad:** el precio y el stock no pueden quedar en un estado inválido desde fuera de la clase.
* **Cohesión:** las reglas del producto viven en el archivo del producto.
* **Mantenibilidad:** si una regla cambia, se modifica en un solo lugar.
* **Testabilidad:** el dominio se prueba sin base de datos ni API.

---

## 6. Entidades y relaciones

| Entidad | Rol en el dominio | Relaciones |
| --- | --- | --- |
| **`Product`** | Entidad principal (agregado). Representa el artículo en venta | Pertenece a 1 `Category` y 1 `Brand`. Contiene N `Reviews` |
| **`Category`** | Clasificación del catálogo | Tiene N `Products` |
| **`Brand`** | Marca fabricante | Tiene N `Products` |
| **`Review`** | Calificación dejada por un cliente | Pertenece a 1 `Product` |

**Cómo se expresan las relaciones en Product**
```csharp
public Guid CategoryId { get; private set; } // clave foránea
public Guid BrandId { get; private set; } // clave foránea

public Category? Category { get; private set; } // propiedad de navegación
public Brand? Brand { get; private set; } // propiedad de navegación
public ICollection<Review> Reviews { get; private set; } = new List<Review>();
```
* Se usan `Guid` como identificadores: se pueden generar en el código sin consultar la base de datos.
* Las propiedades de navegación son `nullable (?)` porque no están cargadas hasta que un ORM las resuelve.
* El par `CategoryId` + `Category` es la convención que EF Core reconocerá más adelante para mapear la relación sin configuración adicional.

---

## 7. Reglas de negocio encapsuladas
Todo el estado de `Product` está protegido con `private set`. La única forma de cambiarlo es mediante métodos de negocio explícitos.

### 7.1 Constructor defensivo
El constructor público no asigna directamente precio ni stock: reutiliza los métodos de la clase, de modo que las reglas existen en un único lugar.
```csharp
public Product(Guid id, string name, decimal price, int stock, Guid categoryId, Guid brandId)
{
    Id = id;
    Name = name;
    CategoryId = categoryId;
    BrandId = brandId;
    
    UpdatePrice(price); // Valida el precio inicial
    AddStock(stock); // Valida el stock inicial
}
```

### 7.2 Constructor protected
```csharp
protected Product() { }
```
Existe únicamente para que un ORM como Entity Framework pueda materializar la entidad. Al no ser público, el código de aplicación no puede crear un `Product` sin pasar por el constructor defensivo.

### 7.3 Comportamientos y excepciones
| Método | Regla aplicada | Excepción si falla |
| --- | --- | --- |
| `UpdatePrice(decimal)` | El nuevo precio no puede ser menor a 0 | `ArgumentException` |
| `AddStock(int)` | La cantidad a sumar no puede ser negativa | `ArgumentException` |
| `RemoveStock(int)` | La cantidad a restar debe ser mayor a 0 | `ArgumentException` |
| `RemoveStock(int)` | El stock actual debe ser mayor o igual a la cantidad solicitada | `InvalidOperationException` |

```csharp
public void UpdatePrice(decimal newPrice)
{
    if (newPrice < 0) throw new ArgumentException("El precio no puede ser negativo.");
    Price = newPrice;
}

public void AddStock(int quantity)
{
    if (quantity < 0) throw new ArgumentException("La cantidad a agregar no puede ser negativa.");
    Stock += quantity;
}

public void RemoveStock(int quantity)
{
    if (quantity <= 0) throw new ArgumentException("La cantidad a retirar debe ser mayor a cero.");
    if (Stock < quantity) throw new InvalidOperationException("No hay suficiente inventario para realizar esta operación.");
    Stock -= quantity;
}
```

### 7.4 Criterio para elegir la excepción
| Excepción | Cuándo se usa | Ejemplo |
| --- | --- | --- |
| `ArgumentException` | El argumento es inválido por sí mismo, sin importar el estado del objeto | Precio -5 |
| `InvalidOperationException` | El argumento es válido, pero la operación no es posible dado el estado actual | Retirar 10 unidades con stock de 3 |

---

## 8. Ejemplos de uso
```csharp
var product = new Product(
    id: Guid.NewGuid(),
    name: "Teclado mecánico",
    price: 1200m,
    stock: 10,
    categoryId: Guid.NewGuid(),
    brandId: Guid.NewGuid());

product.RemoveStock(3);       // Stock = 7
product.UpdatePrice(999.90m); // Price = 999.90

product.RemoveStock(50);      // InvalidOperationException: no hay suficiente inventario
product.UpdatePrice(-1m);     // ArgumentException: el precio no puede ser negativo

product.Stock = 100;          // ERROR DE COMPILACIÓN: el setter es privado
```
*La última línea muestra el punto clave de la rama: el compilador impide saltarse las reglas.*

---

## 9. Decisiones de diseño y limitaciones conocidas
Estas son las reglas que **no** están implementadas todavía. Se listan para que el alcance real del dominio quede claro:

| Tema | Comportamiento actual | Posible mejora |
| --- | --- | --- |
| `AddStock(0)` | Se acepta (solo se rechazan negativos) | Exigir cantidad mayor a 0 para igualar a `RemoveStock` |
| `Name` | No se valida: acepta vacío o `null` | Rechazar nombres vacíos o en blanco |
| `Description` | Tiene `private set`, pero ningún método ni el constructor permite asignarla | Agregar `UpdateDescription(string)` |
| `Name` posterior a la creación | No hay forma de cambiarlo | Agregar `Rename(string)` con su validación |
| `categoryId` / `brandId` | No se valida que sean distintos de `Guid.Empty` | Validar en el constructor |
| `Reviews` | Es un `ICollection` expuesto con `private set`, pero se puede llamar a `.Add` sobre la colección desde fuera | Exponer `IReadOnlyCollection<Review>` y agregar un método `AddReview` |

*Por esto, la afirmación "es imposible dejar un Producto en estado inválido" aplica hoy al precio y al stock, que son las reglas implementadas. El resto de las invariantes están pendientes.*

---

## 10. Verificación
```bash
dotnet build ProductAPI.Domain/ProductAPI.Domain.csproj
```
Para confirmar que el dominio no tiene dependencias externas:
```bash
dotnet list ProductAPI.Domain/ProductAPI.Domain.csproj package
dotnet list ProductAPI.Domain/ProductAPI.Domain.csproj reference
```
Ambos comandos deben indicar que no hay paquetes ni referencias a proyectos.

---

## 11. Próximos pasos
* **Pruebas unitarias:** crear `ProductAPI.Domain.Tests` con xUnit para verificar que las reglas lanzan las excepciones correctas (ver rama `main`).
* **Cerrar las limitaciones** de la sección 9.
* **Persistencia:** configurar Entity Framework Core para mapear este modelo a tablas relacionales sin contaminar el dominio (ver rama `feature/ef-core-sqlserver`).
