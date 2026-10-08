using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Categories.Commands.DeleteCategory;
public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand> {
    private readonly ICategoryRepository _repository;
    public DeleteCategoryCommandHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task Handle(DeleteCategoryCommand request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Category", request.Id);
        _repository.Delete(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}

