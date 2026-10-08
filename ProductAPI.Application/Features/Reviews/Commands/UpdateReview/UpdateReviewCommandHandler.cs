using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Reviews.Commands.UpdateReview;
public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand> {
    private readonly IReviewRepository _repository;
    public UpdateReviewCommandHandler(IReviewRepository repository) { _repository = repository; }
    public async Task Handle(UpdateReviewCommand request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Review", request.Id);
        entity.GetType().GetProperty("Comment").SetValue(entity, request.Comment); entity.GetType().GetProperty("Rating").SetValue(entity, request.Rating);
        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
