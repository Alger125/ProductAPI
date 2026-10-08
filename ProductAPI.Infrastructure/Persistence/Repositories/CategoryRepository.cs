using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class CategoryRepository : ICategoryRepository {
    private readonly ApplicationDbContext _context;
    public CategoryRepository(ApplicationDbContext context) { _context = context; }
    
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
        return await _context.Set<Category>().FindAsync(new object[] { id }, cancellationToken);
    }
    public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _context.Set<Category>().ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Category entity, CancellationToken cancellationToken = default) {
        await _context.Set<Category>().AddAsync(entity, cancellationToken);
    }
    public void Update(Category entity) {
        _context.Set<Category>().Update(entity);
    }
    public void Delete(Category entity) {
        _context.Set<Category>().Remove(entity);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
