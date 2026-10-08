$baseDir = "C:\Users\Jon Jimz\.gemini\antigravity\scratch\ProductAPI"

# --- Brands ---
mkdir -Force "$baseDir\ProductAPI.Application\Features\Brands\Commands\CreateBrand" | Out-Null
mkdir -Force "$baseDir\ProductAPI.Application\Features\Brands\Queries\GetBrands" | Out-Null

Set-Content -Path "$baseDir\ProductAPI.Application\Repositories\IBrandRepository.cs" -Value @"
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface IBrandRepository {
    Task<IEnumerable<Brand>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Brand brand, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
"@

Set-Content -Path "$baseDir\ProductAPI.Infrastructure\Persistence\Repositories\BrandRepository.cs" -Value @"
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
"@

Set-Content -Path "$baseDir\ProductAPI.Application\DTOs\BrandDto.cs" -Value @"
namespace ProductAPI.Application.DTOs;
public record BrandDto(Guid Id, string Name, string Country);
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Brands\Commands\CreateBrand\CreateBrandCommand.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.Brands.Commands.CreateBrand;
public record CreateBrandCommand(string Name, string Country) : IRequest<Guid>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Brands\Commands\CreateBrand\CreateBrandCommandHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Features.Brands.Commands.CreateBrand;
public class CreateBrandCommandHandler : IRequestHandler<CreateBrandCommand, Guid> {
    private readonly IBrandRepository _repository;
    public CreateBrandCommandHandler(IBrandRepository repository) { _repository = repository; }
    public async Task<Guid> Handle(CreateBrandCommand request, CancellationToken cancellationToken) {
        var brand = new Brand { Id = Guid.NewGuid(), Name = request.Name, Country = request.Country };
        await _repository.AddAsync(brand, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return brand.Id;
    }
}
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Brands\Queries\GetBrands\GetBrandsQuery.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Brands.Queries.GetBrands;
public record GetBrandsQuery() : IRequest<IEnumerable<BrandDto>>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Brands\Queries\GetBrands\GetBrandsQueryHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
namespace ProductAPI.Application.Features.Brands.Queries.GetBrands;
public class GetBrandsQueryHandler : IRequestHandler<GetBrandsQuery, IEnumerable<BrandDto>> {
    private readonly IBrandRepository _repository;
    public GetBrandsQueryHandler(IBrandRepository repository) { _repository = repository; }
    public async Task<IEnumerable<BrandDto>> Handle(GetBrandsQuery request, CancellationToken cancellationToken) {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new BrandDto(e.Id, e.Name, e.Country));
    }
}
"@

Set-Content -Path "$baseDir\ProductAPI.Api\Controllers\BrandsController.cs" -Value @"
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Brands.Commands.CreateBrand;
using ProductAPI.Application.Features.Brands.Queries.GetBrands;
namespace ProductAPI.Api.Controllers;
[ApiController]
[Route(`"api/[controller]`")]
public class BrandsController : ControllerBase {
    private readonly IMediator _mediator;
    public BrandsController(IMediator mediator) { _mediator = mediator; }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateBrandCommand command) => Ok(new { Id = await _mediator.Send(command) });
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetBrandsQuery()));
}
"@


# --- Categories ---
mkdir -Force "$baseDir\ProductAPI.Application\Features\Categories\Commands\CreateCategory" | Out-Null
mkdir -Force "$baseDir\ProductAPI.Application\Features\Categories\Queries\GetCategories" | Out-Null

Set-Content -Path "$baseDir\ProductAPI.Application\Repositories\ICategoryRepository.cs" -Value @"
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface ICategoryRepository {
    Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Category category, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
"@

Set-Content -Path "$baseDir\ProductAPI.Infrastructure\Persistence\Repositories\CategoryRepository.cs" -Value @"
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
"@

Set-Content -Path "$baseDir\ProductAPI.Application\DTOs\CategoryDto.cs" -Value @"
namespace ProductAPI.Application.DTOs;
public record CategoryDto(Guid Id, string Name, string Description);
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Categories\Commands\CreateCategory\CreateCategoryCommand.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.Categories.Commands.CreateCategory;
public record CreateCategoryCommand(string Name, string Description) : IRequest<Guid>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Categories\Commands\CreateCategory\CreateCategoryCommandHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Features.Categories.Commands.CreateCategory;
public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid> {
    private readonly ICategoryRepository _repository;
    public CreateCategoryCommandHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken) {
        var category = new Category { Id = Guid.NewGuid(), Name = request.Name, Description = request.Description };
        await _repository.AddAsync(category, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return category.Id;
    }
}
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Categories\Queries\GetCategories\GetCategoriesQuery.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Categories.Queries.GetCategories;
public record GetCategoriesQuery() : IRequest<IEnumerable<CategoryDto>>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Categories\Queries\GetCategories\GetCategoriesQueryHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
namespace ProductAPI.Application.Features.Categories.Queries.GetCategories;
public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>> {
    private readonly ICategoryRepository _repository;
    public GetCategoriesQueryHandler(ICategoryRepository repository) { _repository = repository; }
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken) {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new CategoryDto(e.Id, e.Name, e.Description));
    }
}
"@

Set-Content -Path "$baseDir\ProductAPI.Api\Controllers\CategoriesController.cs" -Value @"
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Categories.Commands.CreateCategory;
using ProductAPI.Application.Features.Categories.Queries.GetCategories;
namespace ProductAPI.Api.Controllers;
[ApiController]
[Route(`"api/[controller]`")]
public class CategoriesController : ControllerBase {
    private readonly IMediator _mediator;
    public CategoriesController(IMediator mediator) { _mediator = mediator; }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command) => Ok(new { Id = await _mediator.Send(command) });
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetCategoriesQuery()));
}
"@


# --- Reviews ---
mkdir -Force "$baseDir\ProductAPI.Application\Features\Reviews\Commands\CreateReview" | Out-Null
mkdir -Force "$baseDir\ProductAPI.Application\Features\Reviews\Queries\GetReviews" | Out-Null

Set-Content -Path "$baseDir\ProductAPI.Application\Repositories\IReviewRepository.cs" -Value @"
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface IReviewRepository {
    Task<IEnumerable<Review>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(Review review, CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
"@

Set-Content -Path "$baseDir\ProductAPI.Infrastructure\Persistence\Repositories\ReviewRepository.cs" -Value @"
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
"@

Set-Content -Path "$baseDir\ProductAPI.Application\DTOs\ReviewDto.cs" -Value @"
namespace ProductAPI.Application.DTOs;
public record ReviewDto(Guid Id, Guid ProductId, string ReviewerName, int Rating, string Comment, DateTime CreatedAt);
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Reviews\Commands\CreateReview\CreateReviewCommand.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.Reviews.Commands.CreateReview;
public record CreateReviewCommand(Guid ProductId, string ReviewerName, int Rating, string Comment) : IRequest<Guid>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Reviews\Commands\CreateReview\CreateReviewCommandHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Features.Reviews.Commands.CreateReview;
public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid> {
    private readonly IReviewRepository _repository;
    public CreateReviewCommandHandler(IReviewRepository repository) { _repository = repository; }
    public async Task<Guid> Handle(CreateReviewCommand request, CancellationToken cancellationToken) {
        var review = new Review { Id = Guid.NewGuid(), ProductId = request.ProductId, ReviewerName = request.ReviewerName, Rating = request.Rating, Comment = request.Comment, CreatedAt = DateTime.UtcNow };
        await _repository.AddAsync(review, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);
        return review.Id;
    }
}
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Reviews\Queries\GetReviews\GetReviewsQuery.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.Reviews.Queries.GetReviews;
public record GetReviewsQuery() : IRequest<IEnumerable<ReviewDto>>;
"@

Set-Content -Path "$baseDir\ProductAPI.Application\Features\Reviews\Queries\GetReviews\GetReviewsQueryHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
namespace ProductAPI.Application.Features.Reviews.Queries.GetReviews;
public class GetReviewsQueryHandler : IRequestHandler<GetReviewsQuery, IEnumerable<ReviewDto>> {
    private readonly IReviewRepository _repository;
    public GetReviewsQueryHandler(IReviewRepository repository) { _repository = repository; }
    public async Task<IEnumerable<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken) {
        var entities = await _repository.GetAllAsync(cancellationToken);
        return entities.Select(e => new ReviewDto(e.Id, e.ProductId, e.ReviewerName, e.Rating, e.Comment, e.CreatedAt));
    }
}
"@

Set-Content -Path "$baseDir\ProductAPI.Api\Controllers\ReviewsController.cs" -Value @"
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.Reviews.Commands.CreateReview;
using ProductAPI.Application.Features.Reviews.Queries.GetReviews;
namespace ProductAPI.Api.Controllers;
[ApiController]
[Route(`"api/[controller]`")]
public class ReviewsController : ControllerBase {
    private readonly IMediator _mediator;
    public ReviewsController(IMediator mediator) { _mediator = mediator; }
    [HttpPost] public async Task<IActionResult> Create([FromBody] CreateReviewCommand command) => Ok(new { Id = await _mediator.Send(command) });
    [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new GetReviewsQuery()));
}
"@
