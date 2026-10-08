using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;

namespace ProductAPI.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Set<Product>()
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await _context.Set<Product>().AddAsync(product, cancellationToken);
    }

    public void Update(Product product)
    {
        _context.Set<Product>().Update(product);
    }

    public void Delete(Product product)
    {
        _context.Set<Product>().Remove(product);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
