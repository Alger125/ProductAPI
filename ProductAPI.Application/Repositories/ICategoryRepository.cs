using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface ICategoryRepository {
    Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Category entity, CancellationToken cancellationToken = default);
    void Update(Category entity);
    void Delete(Category entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
