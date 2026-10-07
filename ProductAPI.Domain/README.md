# Product Domain

Rama:

eature/product-domain

## Objetivo

Crear el núcleo de negocio de ProductAPI manteniendo la capa de dominio independiente de bases de datos, APIs y otros componentes externos.

## Entidades creadas

Se implementaron las siguientes entidades:

- Product
- Category
- Brand
- Review

Estas clases se encuentran dentro de:

`	ext
ProductAPI.Domain/Entities
`

## Product

La entidad Product contiene las principales reglas de negocio relacionadas con productos e inventario.

Sus propiedades utilizan private set para evitar modificaciones directas desde otras capas.

Ejemplo:

`csharp
public decimal Price { get; private set; }
public int Stock { get; private set; }
`

## Reglas de negocio

### UpdatePrice

Permite modificar el precio del producto.

No permite precios negativos.

`csharp
product.UpdatePrice(1500);
`

### AddStock

Permite agregar unidades al inventario.

No permite cantidades negativas.

### RemoveStock

Permite retirar unidades del inventario.

Valida que:

- La cantidad sea mayor a cero.
- Exista suficiente inventario.

## Entity Framework Core

La entidad Product incluye un constructor protegido:

`csharp
protected Product() { }
`

Este constructor permitirá que Entity Framework Core pueda crear objetos al recuperar información de la base de datos.

## Pruebas unitarias

Se creó el proyecto:

`	ext
ProductAPI.Domain.Tests
`

utilizando **xUnit**.

Actualmente se comprueba que:

- Un precio negativo genera una excepción.
- Un precio válido actualiza correctamente el producto.

## Estado

**COMPLETADA**

El dominio está listo para continuar con la integración de persistencia mediante Entity Framework Core y SQL Server.
