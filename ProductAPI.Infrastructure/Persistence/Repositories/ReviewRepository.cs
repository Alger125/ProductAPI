using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class ReviewRepository : IReviewRepository {
    private readonly ApplicationDbContext _context;
    public ReviewRepository(ApplicationDbContext context) { _context = context; }
    public async Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Set<Review>().ToListAsync(cancellationToken);
    public async Task AddAsync(Review review, CancellationToken cancellationToken = default) => await _context.Set<Review>().AddAsync(review, cancellationToken);
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
}
