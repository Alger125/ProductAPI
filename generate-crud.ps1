$baseDir = "C:\Users\Jon Jimz\.gemini\antigravity\scratch\ProductAPI"

# --- GetProductById ---
mkdir -Force "$baseDir\ProductAPI.Application\Features\Products\Queries\GetProductById" | Out-Null

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Products\Queries\GetProductById\GetProductByIdQuery.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Products.Queries.GetProductById;
public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Products\Queries\GetProductById\GetProductByIdQueryHandler.cs" -Value @"
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
"@


# --- UpdateProduct ---
mkdir -Force "$baseDir\ProductAPI.Application\Features\Products\Commands\UpdateProduct" | Out-Null

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Products\Commands\UpdateProduct\UpdateProductCommand.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.Products.Commands.UpdateProduct;
public record UpdateProductCommand(Guid Id, decimal Price, int StockToAdd) : IRequest;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Products\Commands\UpdateProduct\UpdateProductCommandHandler.cs" -Value @"
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
"@


# --- DeleteProduct ---
mkdir -Force "$baseDir\ProductAPI.Application\Features\Products\Commands\DeleteProduct" | Out-Null

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Products\Commands\DeleteProduct\DeleteProductCommand.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.Products.Commands.DeleteProduct;
public record DeleteProductCommand(Guid Id) : IRequest;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Products\Commands\DeleteProduct\DeleteProductCommandHandler.cs" -Value @"
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
"@
