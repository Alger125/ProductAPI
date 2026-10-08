using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Categories.Queries.GetCategories;
public record GetCategoriesQuery() : IRequest<IEnumerable<CategoryDto>>;
