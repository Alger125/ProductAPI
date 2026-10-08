using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Categories.Queries.GetCategoryById;
public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryDto> {
    private readonly ICategoryRepository _repository;
    public GetCategoryByIdQueryHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Category", request.Id);
        return new CategoryDto(entity.Id, entity.Name, entity.Description);
    }
}

