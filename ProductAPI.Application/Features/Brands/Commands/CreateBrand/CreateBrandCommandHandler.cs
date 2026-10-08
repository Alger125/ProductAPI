using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Features.Brands.Commands.CreateBrand;
public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid> {
    private readonly IBrandRepository _repository;
    public CreateBrandCommandHandler(IBrandRepository repository) { _repository = repository; }
    public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken) {
        var brand = new Brand { Id = Guid.NewGuid(), Name = request.Name, Country = request.Country };
        await _repository.AddAsync(brand, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return brand.Id;
    }
}
