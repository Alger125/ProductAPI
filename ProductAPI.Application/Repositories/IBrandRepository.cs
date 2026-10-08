using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface IBrandRepository {
    Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Brand brand, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
