using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _productRepository;

    public CreateProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // 1. Instanciamos nuestra Entidad de Dominio usando el Constructor Seguro
        // (Esto automáticamente validará que el precio y el stock no sean negativos)
        var product = new Product(
            Guid.NewGuid(),
            request.Name,
            request.Price,
            request.Stock,
            request.CategoryId,
            request.BrandId
        );

        // TODO: Mapear la descripción cuando tengamos un método para ello en el dominio

        // 2. Usamos el repositorio para agregarlo
        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);

        // 3. Devolvemos el Id del producto recién creado
        return product.Id;
    }
}
