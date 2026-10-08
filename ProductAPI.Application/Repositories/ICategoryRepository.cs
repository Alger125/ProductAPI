using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface ICategoryRepository {
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
