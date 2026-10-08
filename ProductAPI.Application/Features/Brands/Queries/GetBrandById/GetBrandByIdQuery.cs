using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Brands.Queries.GetBrandById;
public record GetBrandByIdQuery(Guid Id) : IRequest<BrandDto>;
