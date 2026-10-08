using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Products.Commands.DeleteProduct;
public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand> {
    private readonly IProductRepository _repository;
    public DeleteProductCommandHandler(IProductRepository repository) { _repository = repository; }
    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken) {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null) throw new NotFoundException("Product", request.Id);
        _repository.Delete(product);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
