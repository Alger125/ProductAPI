using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class ReviewRepository : IReviewRepository {
    private readonly ApplicationDbContext _context;
    public ReviewRepository(ApplicationDbContext context) { _context = context; }
    
    public async Task<Review?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
        return await _context.Set<Review>().FindAsync(new object[] { id }, cancellationToken);
    }
    public async Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _context.Set<Review>().ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Review entity, CancellationToken cancellationToken = default) {
        await _context.Set<Review>().AddAsync(entity, cancellationToken);
    }
    public void Update(Review entity) {
        _context.Set<Review>().Update(entity);
    }
    public void Delete(Review entity) {
        _context.Set<Review>().Remove(entity);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
