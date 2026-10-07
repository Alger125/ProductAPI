using System;
using System.Collections.Generic;

namespace ProductAPI.Domain.Entities;

public class Product
{
    // Cambiamos 'set' por 'private set' para proteger los datos
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

    // Constructor vacío requerido por Entity Framework Core
    protected Product() { }

    // Constructor para crear un producto válido desde el inicio
    public Product(Guid id, string name, decimal price, int stock, Guid categoryId, Guid brandId)
    {
        Id = id;
        Name = name;
        CategoryId = categoryId;
        BrandId = brandId;
        
        UpdatePrice(price); // Usamos nuestro propio método para validar desde la creación
        AddStock(stock);    // Validamos el stock inicial
    }

    // ==========================================
    // REGLAS DE NEGOCIO (Comportamiento)
    // ==========================================

    public void UpdatePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentException("El precio no puede ser negativo.");
        
        Price = newPrice;
    }

    public void AddStock(int quantity)
    {
        if (quantity < 0)
            throw new ArgumentException("La cantidad a agregar no puede ser negativa.");
        
        Stock += quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("La cantidad a retirar debe ser mayor a cero.");
            
        if (Stock < quantity)
            throw new InvalidOperationException("No hay suficiente inventario para realizar esta operación.");
            
        Stock -= quantity;
    }
}
