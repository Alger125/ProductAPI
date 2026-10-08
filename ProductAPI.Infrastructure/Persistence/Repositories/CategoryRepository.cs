using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class CategoryRepository : ICategoryRepository {
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context) { _context = context; }
    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Set<Category>().ToListAsync(cancellationToken);
    public async Task AddAsync(Category category, CancellationToken cancellationToken = default) => await _context.Set<Category>().AddAsync(category, cancellationToken);
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
}
