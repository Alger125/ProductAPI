using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface IReviewRepository {
    Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Review entity, CancellationToken cancellationToken = default);
    void Update(Review entity);
    void Delete(Review entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
