using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class BrandRepository : IBrandRepository {
    private readonly ApplicationDbContext _context;
    public BrandRepository(ApplicationDbContext context) { _context = context; }
    public async Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default) => await _context.Set<Brand>().ToListAsync(cancellationToken);
    public async Task AddAsync(Brand brand, CancellationToken cancellationToken = default) => await _context.Set<Brand>().AddAsync(brand, cancellationToken);
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => await _context.SaveChangesAsync(cancellationToken);
}
