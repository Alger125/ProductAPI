using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Features.Reviews.Commands.CreateReview;
public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid> {
    private readonly IReviewRepository _repository;
    public CreateReviewCommandHandler(IReviewRepository repository) { _repository = repository; }
    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken) {
        var review = new Review { Id = Guid.NewGuid(), ProductId = request.ProductId, ReviewerName = request.ReviewerName, Rating = request.Rating, Comment = request.Comment, CreatedAt = DateTime.UtcNow };
        await _repository.AddAsync(review, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return review.Id;
    }
}
