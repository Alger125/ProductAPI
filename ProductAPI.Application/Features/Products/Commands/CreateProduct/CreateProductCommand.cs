using MediatR;

namespace ProductAPI.Application.Features.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    int Stock,
    Guid CategoryId,
    Guid BrandId) : IRequest<Guid>;
