using MediatR;
namespace ProductAPI.Application.Features.Products.Commands.UpdateProduct;
public record UpdateProductCommand(Guid Id, decimal Price, int StockToAdd) : IRequest;
