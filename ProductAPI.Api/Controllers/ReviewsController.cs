using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Reviews.Commands.CreateReview;
using ProductAPI.Application.Features.Reviews.Queries.GetReviews;
namespace ProductAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase {
    private readonly IMediator _mediator;
    public ReviewsController(IMediator mediator) { _mediator = mediator; }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateReviewCommand command) => Ok(new { Id = await _mediator.Send(command) });
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetReviewsQuery()));
}
