using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Reviews.Queries.GetReviewById;
public class GetReviewByIdQueryHandler : IRequestHandler<GetReviewByIdQuery, ReviewDto> {
    private readonly IReviewRepository _repository;
    public GetReviewByIdQueryHandler(IReviewRepository repository) { _repository = repository; }
    public async Task<ReviewDto> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Review", request.Id);
        return new ReviewDto(entity.Id, entity.ProductId, entity.ReviewerName, entity.Rating, entity.Comment, entity.CreatedAt);
    }
}
