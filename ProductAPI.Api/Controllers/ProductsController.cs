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
        var productId = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProductById), new { id = productId }, new { Message = "Producto creado con éxito", ProductId = productId });
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var query = new ProductAPI.Application.Features.Products.Queries.GetProducts.GetProductsQuery();
        var products = await _mediator.Send(query);
        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var query = new ProductAPI.Application.Features.Products.Queries.GetProductById.GetProductByIdQuery(id);
        var product = await _mediator.Send(query);
        return Ok(product);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] ProductAPI.Application.Features.Products.Commands.UpdateProduct.UpdateProductCommand command)
    {
        if (id != command.Id) return BadRequest("El ID de la ruta no coincide con el del cuerpo.");
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(Guid id)
    {
        var command = new ProductAPI.Application.Features.Products.Commands.DeleteProduct.DeleteProductCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}

