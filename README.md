# ProductAPI - Rama feature/product-domain

Implementación inicial del núcleo del negocio (Core Domain) siguiendo los principios de Rich Domain Model y Clean Architecture en .NET 8.

## Tabla de contenidos
1. [Objetivo de la rama](#1-objetivo-de-la-rama)
2. [Arquitectura y Estructura](#2-arquitectura-y-estructura)
3. [Diseño del Dominio (Rich Domain)](#3-diseño-del-dominio-rich-domain)
4. [Entidades y Relaciones](#4-entidades-y-relaciones)
5. [Reglas de Negocio Encapsuladas](#5-reglas-de-negocio-encapsuladas)
6. [Próximos pasos](#6-próximos-pasos)

---

## 1. Objetivo de la rama
El objetivo principal de esta rama es diseñar el "corazón" de la aplicación: la capa de **Dominio**. Aquí se definen las entidades principales del negocio (`Product`, `Brand`, `Category`, `Review`) de forma pura, sin depender de ninguna base de datos, framework externo o capa de presentación.

En lugar de crear un dominio anémico (clases que solo son bolsas de datos con *getters* y *setters*), se implementó un **Rich Domain Model** donde las reglas de negocio viven y se protegen dentro de las propias entidades.

---

## 2. Arquitectura y Estructura
```text
ProductAPI.Domain/
├── ProductAPI.Domain.csproj
└── Entities/
    ├── Brand.cs
    ├── Category.cs
    ├── Product.cs
    └── Review.cs
```

**Regla de Dependencias:** La capa `ProductAPI.Domain` no tiene referencias a ningún otro proyecto ni a paquetes NuGet externos (`Microsoft.EntityFrameworkCore`, etc.). Es código C# puro.

---

## 3. Diseño del Dominio (Rich Domain)

### ¿Por qué un Dominio Rico?
En un proyecto tradicional, la lógica de validación de un producto (ej. "el precio no puede ser negativo") suele escribirse en los Servicios o Controladores. En esta rama, hemos movido esa responsabilidad a la propia entidad `Product`.

**Beneficios logrados:**
1. **Seguridad:** Es imposible crear o modificar un Producto dejándolo en un estado inválido.
2. **Cohesión:** Las reglas del producto viven dentro del archivo del producto.
3. **Mantenibilidad:** Si la regla cambia, solo se modifica en un solo lugar.

---

## 4. Entidades y Relaciones

| Entidad | Rol en el Dominio | Relaciones |
| --- | --- | --- |
| **`Product`** | Entidad principal (Agregado). Representa el artículo en venta. | Pertenece a 1 `Category` y 1 `Brand`. Contiene N `Reviews`. |
| **`Category`** | Clasificación del catálogo. | Tiene N `Products`. |
| **`Brand`** | Marca fabricante. | Tiene N `Products`. |
| **`Review`** | Calificación dejada por un cliente. | Pertenece a 1 `Product`. |

---

## 5. Reglas de Negocio Encapsuladas

Todo el estado de la entidad `Product` está protegido mediante propiedades `private set`. La única forma de interactuar con los datos es a través de métodos de negocio explícitos.

### Constructor Defensivo
Para crear un Producto, se debe usar su constructor público, el cual valida los datos iniciales utilizando los métodos de la propia clase:
```csharp
public Product(Guid id, string name, decimal price, int stock, Guid categoryId, Guid brandId)
{
    Id = id;
    Name = name;
    CategoryId = categoryId;
    BrandId = brandId;
    
    UpdatePrice(price); // Valida el precio inicial
    AddStock(stock);    // Valida el stock inicial
}
```
*(Nota: Se incluye un constructor `protected` vacío exclusivamente para requerimientos de materialización de futuros ORMs como Entity Framework).*

### Comportamientos y Excepciones
| Acción (Método) | Regla aplicada | Excepción lanzada si falla |
| --- | --- | --- |
| `UpdatePrice(decimal)` | El nuevo precio no puede ser menor a 0. | `ArgumentException` |
| `AddStock(int)` | La cantidad a sumar debe ser positiva. | `ArgumentException` |
| `RemoveStock(int)` | La cantidad a restar debe ser positiva. | `ArgumentException` |
| `RemoveStock(int)` | El stock actual debe ser mayor o igual a la cantidad solicitada. | `InvalidOperationException` |

Ejemplo de implementación interna:
```csharp
public void RemoveStock(int quantity)
{
    if (quantity <= 0)
        throw new ArgumentException("La cantidad a retirar debe ser mayor a cero.");
        
    if (Stock < quantity)
        throw new InvalidOperationException("No hay suficiente inventario para realizar esta operación.");
        
    Stock -= quantity;
}
```

---

## 6. Próximos pasos
1. **Pruebas Unitarias:** Crear el proyecto `ProductAPI.Domain.Tests` para validar matemáticamente con xUnit que estas reglas arrojan las excepciones correctas (Ver rama `main`).
2. **Persistencia:** Configurar Entity Framework Core para mapear este modelo puro a tablas relacionales sin contaminar el dominio (Ver rama `feature/ef-core-sqlserver`).
