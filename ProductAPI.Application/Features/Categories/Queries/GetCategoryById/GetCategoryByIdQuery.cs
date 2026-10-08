using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Categories.Queries.GetCategoryById;
public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;

