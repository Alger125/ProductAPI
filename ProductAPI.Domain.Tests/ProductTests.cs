using ProductAPI.Domain.Entities;
using System;
using Xunit;

namespace ProductAPI.Domain.Tests;

public class ProductTests
{
    [Fact] // Esta etiqueta le dice a .NET que este método es una prueba
    public void UpdatePrice_Should_ThrowException_When_PriceIsNegative()
    {
        // 1. Arrange (Preparar)
        // Creamos un producto válido inicialmente
        var product = new Product(Guid.NewGuid(), "Laptop", 1000m, 10, Guid.NewGuid(), Guid.NewGuid());

        // 2. Act (Actuar) & 3. Assert (Afirmar/Comprobar)
        // Intentamos ponerle un precio negativo y afirmamos que DEBE arrojar un ArgumentException
        Assert.Throws<ArgumentException>(() => product.UpdatePrice(-50m));
    }

    [Fact]
    public void UpdatePrice_Should_UpdateValue_When_PriceIsValid()
    {
        // 1. Arrange
        var product = new Product(Guid.NewGuid(), "Laptop", 1000m, 10, Guid.NewGuid(), Guid.NewGuid());

        // 2. Act
        product.UpdatePrice(1500m);

        // 3. Assert
        Assert.Equal(1500m, product.Price);
    }
}