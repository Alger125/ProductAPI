using MediatR;
namespace ProductAPI.Application.Features.Reviews.Commands.DeleteReview;
public record DeleteReviewCommand(Guid Id) : IRequest;
