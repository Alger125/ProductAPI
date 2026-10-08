using MediatR;
namespace ProductAPI.Application.Features.Reviews.Commands.CreateReview;
public record CreateReviewCommand(Guid ProductId, string ReviewerName, int Rating, string Comment) : IRequest<Guid>;
