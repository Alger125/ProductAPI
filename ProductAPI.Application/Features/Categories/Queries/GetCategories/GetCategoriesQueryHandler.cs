using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
namespace ProductAPI.Application.Features.Categories.Queries.GetCategories;
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>> {
    private readonly ICategoryRepository _repository;
    public GetCategoriesQueryHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken) {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new CategoryDto(e.Id, e.Name, e.Description));
    }
}
