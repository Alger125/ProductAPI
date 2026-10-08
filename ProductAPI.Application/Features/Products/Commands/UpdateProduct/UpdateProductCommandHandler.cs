using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Products.Commands.UpdateProduct;
public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand> {
    private readonly IProductRepository _repository;
    public UpdateProductCommandHandler(IProductRepository repository) { _repository = repository; }
    public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken) {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null) throw new NotFoundException("Product", request.Id);
        product.UpdatePrice(request.Price);
        if (request.StockToAdd > 0) product.AddStock(request.StockToAdd);
        _repository.Update(product);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
