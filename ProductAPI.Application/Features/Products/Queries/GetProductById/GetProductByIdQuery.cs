using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Products.Queries.GetProductById;
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
