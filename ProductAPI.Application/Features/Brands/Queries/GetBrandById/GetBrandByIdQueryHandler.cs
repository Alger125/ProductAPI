using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Brands.Queries.GetBrandById;
public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandDto> {
    private readonly IBrandRepository _repository;
    public GetBrandByIdQueryHandler(IBrandRepository repository) { _repository = repository; }
    public async Task<BrandDto> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Brand", request.Id);
        return new BrandDto(entity.Id, entity.Name, entity.Country);
    }
}
