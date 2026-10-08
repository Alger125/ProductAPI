using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Features.Categories.Commands.CreateCategory;
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid> {
    private readonly ICategoryRepository _repository;
    public CreateCategoryCommandHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken) {
        var category = new Category { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description };
        await _repository.AddAsync(category, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}
