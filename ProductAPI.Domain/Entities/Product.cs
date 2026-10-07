using System;
using System.Collections.Generic;

namespace ProductAPI.Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Stock { get; set; }

    // Foreign Keys
    public Guid CategoryId { get; set; }
    public Guid BrandId { get; set; }

    // Navigation properties
    public Category? Category { get; set; }
    public Brand? Brand { get; set; }
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
