using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Reviews.Commands.CreateReview;
using ProductAPI.Application.Features.Reviews.Commands.UpdateReview;
using ProductAPI.Application.Features.Reviews.Commands.DeleteReview;
using ProductAPI.Application.Features.Reviews.Queries.GetReviews;
using ProductAPI.Application.Features.Reviews.Queries.GetReviewById;

namespace ProductAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase {
    private readonly IMediator _mediator;
    public ReviewsController(IMediator mediator) { _mediator = mediator; }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command) {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = id }, new { Id = id });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetReviewsQuery()));
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new GetReviewByIdQuery(id)));
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewCommand command) {
        if (id != command.Id) return BadRequest("Id mismatch");
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) {
        await _mediator.Send(new DeleteReviewCommand(id));
        return NoContent();
    }
}
