using MediatR;
namespace ProductAPI.Application.Features.Products.Commands.DeleteProduct;
public record DeleteProductCommand(Guid Id) : IRequest;
