using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Brands.Commands.UpdateBrand;
public class UpdateBrandCommandHandler : IRequestHandler<UpdateBrandCommand> {
    private readonly IBrandRepository _repository;
    public UpdateBrandCommandHandler(IBrandRepository repository) { _repository = repository; }
    public async Task Handle(UpdateBrandCommand request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Brand", request.Id);
        entity.GetType().GetProperty("Name").SetValue(entity, request.Name);
        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
