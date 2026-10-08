namespace ProductAPI.Application.DTOs;
public record ReviewDto(Guid Id, Guid ProductId, string ReviewerName, int Rating, string Comment, DateTime CreatedAt);
