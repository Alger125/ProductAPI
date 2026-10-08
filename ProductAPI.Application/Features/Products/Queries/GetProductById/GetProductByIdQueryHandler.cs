using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Products.Queries.GetProductById;
public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto> {
    private readonly IProductRepository _repository;
    public GetProductByIdQueryHandler(IProductRepository repository) { _repository = repository; }
    public async Task<ProductDto> Handle(GetProductByIdQuery request, CancellationToken cancellationToken) {
        var product = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (product == null) throw new NotFoundException("Product", request.Id);
        return new ProductDto(product.Id, product.Name, product.Description, product.Price, product.Stock, product.CategoryId, product.BrandId);
    }
}
