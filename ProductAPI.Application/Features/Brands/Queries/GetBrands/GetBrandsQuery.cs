using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Brands.Queries.GetBrands;
public record GetBrandsQuery() : IRequest<IEnumerable<BrandDto>>;
