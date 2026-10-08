using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Brands.Commands.CreateBrand;
using ProductAPI.Application.Features.Brands.Commands.UpdateBrand;
using ProductAPI.Application.Features.Brands.Commands.DeleteBrand;
using ProductAPI.Application.Features.Brands.Queries.GetBrands;
using ProductAPI.Application.Features.Brands.Queries.GetBrandById;

namespace ProductAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase {
    private readonly IMediator _mediator;
    public BrandsController(IMediator mediator) { _mediator = mediator; }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandCommand command) {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = id }, new { Id = id });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetBrandsQuery()));
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new GetBrandByIdQuery(id)));
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBrandCommand command) {
        if (id != command.Id) return BadRequest("Id mismatch");
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) {
        await _mediator.Send(new DeleteBrandCommand(id));
        return NoContent();
    }
}
