using System;

namespace ProductAPI.Domain.Entities;

public class Review
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public string ReviewerName { get; set; } = string.Empty;
    public int Rating { get; set; } // 1 to 5
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    // Navigation property
    public Product? Product { get; set; }
}
