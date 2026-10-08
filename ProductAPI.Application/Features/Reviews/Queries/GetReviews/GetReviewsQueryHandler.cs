using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
namespace ProductAPI.Application.Features.Reviews.Queries.GetReviews;
public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<ReviewDto>> {
    private readonly IReviewRepository _repository;
    public GetReviewsQueryHandler(IReviewRepository repository) { _repository = repository; }
    public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken) {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new ReviewDto(e.Id, e.ProductId, e.ReviewerName, e.Rating, e.Comment, e.CreatedAt));
    }
}
