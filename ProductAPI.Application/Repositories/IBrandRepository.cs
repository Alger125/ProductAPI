using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface IBrandRepository {
    Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Brand entity, CancellationToken cancellationToken = default);
    void Update(Brand entity);
    void Delete(Brand entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
