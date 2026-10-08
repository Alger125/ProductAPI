using MediatR;
namespace ProductAPI.Application.Features.Categories.Commands.DeleteCategory;
public record DeleteCategoryCommand(Guid Id) : IRequest;

