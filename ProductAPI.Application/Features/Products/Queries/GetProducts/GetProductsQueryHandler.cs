using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;

namespace ProductAPI.Application.Features.Products.Queries.GetProducts;

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        // 1. Obtenemos las entidades de la base de datos
        var products = await _productRepository.GetAllAsync(cancellationToken);

        // 2. Las mapeamos a DTOs para no exponer la entidad de dominio directamente a la API
        var productDtos = products.Select(p => new ProductDto(
            p.Id,
            p.Name,
            p.Description,
            p.Price,
            p.Stock,
            p.CategoryId,
            p.BrandId
        ));

        return productDtos;
    }
}
