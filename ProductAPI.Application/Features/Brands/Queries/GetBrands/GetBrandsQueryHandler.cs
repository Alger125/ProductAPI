using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
namespace ProductAPI.Application.Features.Brands.Queries.GetBrands;
public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, IEnumerable<BrandDto>> {
    private readonly IBrandRepository _repository;
    public GetBrandsQueryHandler(IBrandRepository repository) { _repository = repository; }
    public async Task<IEnumerable<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken) {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new BrandDto(e.Id, e.Name, e.Country));
    }
}
