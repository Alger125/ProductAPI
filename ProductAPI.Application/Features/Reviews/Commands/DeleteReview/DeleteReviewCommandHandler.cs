using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Reviews.Commands.DeleteReview;
public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand> {
    private readonly IReviewRepository _repository;
    public DeleteReviewCommandHandler(IReviewRepository repository) { _repository = repository; }
    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Review", request.Id);
        _repository.Delete(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
