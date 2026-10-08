using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Brands.Commands.CreateBrand;
using ProductAPI.Application.Features.Brands.Queries.GetBrands;
namespace ProductAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BrandsController : ControllerBase {
    private readonly IMediator _mediator;
    public BrandsController(IMediator mediator) { _mediator = mediator; }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateBrandCommand command) => StatusCode(StatusCodes.Status201Created, new { Id = await _mediator.Send(command) });
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetBrandsQuery()));
}

