using MediatR;
using ProductAPI.Application.DTOs;

namespace ProductAPI.Application.Features.Products.Queries.GetProducts;

// Una Query no recibe datos (si quisieras paginación, aquí pondrías page y pageSize)
// Y devuelve una colección de ProductDto
public record GetProductsQuery() : IRequest<IEnumerable<ProductDto>>;
