using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Reviews.Queries.GetReviewById;
public record GetReviewByIdQuery(Guid Id) : IRequest<ReviewDto>;
