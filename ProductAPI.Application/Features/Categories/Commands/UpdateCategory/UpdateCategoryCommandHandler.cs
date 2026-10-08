using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Categories.Commands.UpdateCategory;
public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand> {
    private readonly ICategoryRepository _repository;
    public UpdateCategoryCommandHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task Handle(UpdateCategoryCommand request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Category", request.Id);
        entity.GetType().GetProperty("Name").SetValue(entity, request.Name); entity.GetType().GetProperty("Description").SetValue(entity, request.Description);
        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}

