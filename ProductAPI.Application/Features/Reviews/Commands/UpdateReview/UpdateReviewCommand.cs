using MediatR;
namespace ProductAPI.Application.Features.Reviews.Commands.UpdateReview;
public record UpdateReviewCommand(Guid Id, string Comment, int Rating) : IRequest;
