using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface IReviewRepository {
    Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
