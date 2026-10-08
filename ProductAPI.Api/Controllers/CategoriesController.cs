using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Categories.Commands.CreateCategory;
using ProductAPI.Application.Features.Categories.Queries.GetCategories;
namespace ProductAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase {
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) { _mediator = mediator; }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command) => StatusCode(StatusCodes.Status201Created, new { Id = await _mediator.Send(command) });
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetCategoriesQuery()));
}

