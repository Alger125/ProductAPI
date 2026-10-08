using MediatR;
namespace ProductAPI.Application.Features.Categories.Commands.CreateCategory;
public record CreateCategoryCommand(string Name, string Description) : IRequest<Guid>;
