using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Reviews.Queries.GetReviews;
public record GetReviewsQuery() : IRequest<IEnumerable<ReviewDto>>;
