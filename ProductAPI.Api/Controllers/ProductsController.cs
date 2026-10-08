using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Products.Commands.CreateProduct;

namespace ProductAPI.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        // El controlador NO tiene lógica de negocio. Simplemente le pasa el JSON
        // (convertido a comando) a MediatR para que la capa Application lo procese.
        var productId = await _mediator.Send(command);

        return Ok(new { Message = "Producto creado con éxito", ProductId = productId });
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        // Usamos la nueva Query que acabamos de crear
        var query = new ProductAPI.Application.Features.Products.Queries.GetProducts.GetProductsQuery();
        var products = await _mediator.Send(query);

        return Ok(products);
    }
}

