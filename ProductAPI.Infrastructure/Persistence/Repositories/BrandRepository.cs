using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class BrandRepository : IBrandRepository {
    private readonly ApplicationDbContext _context;
    public BrandRepository(ApplicationDbContext context) { _context = context; }
    
    public async Task<Brand?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
        return await _context.Set<Brand>().FindAsync(new object[] { id }, cancellationToken);
    }
    public async Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _context.Set<Brand>().ToListAsync(cancellationToken);
    }
    public async Task AddAsync(Brand entity, CancellationToken cancellationToken = default) {
        await _context.Set<Brand>().AddAsync(entity, cancellationToken);
    }
    public void Update(Brand entity) {
        _context.Set<Brand>().Update(entity);
    }
    public void Delete(Brand entity) {
        _context.Set<Brand>().Remove(entity);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
