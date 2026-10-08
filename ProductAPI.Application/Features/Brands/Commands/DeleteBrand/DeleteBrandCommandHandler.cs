using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.Brands.Commands.DeleteBrand;
public class DeleteBrandCommandHandler : IRequestHandler<DeleteBrandCommand> {
    private readonly IBrandRepository _repository;
    public DeleteBrandCommandHandler(IBrandRepository repository) { _repository = repository; }
    public async Task Handle(DeleteBrandCommand request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("Brand", request.Id);
        _repository.Delete(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
