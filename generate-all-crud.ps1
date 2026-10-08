$entities = @("Brand", "Category", "Review")
$baseDir = "C:\Users\Jon Jimz\.gemini\antigravity\scratch\ProductAPI"

foreach ($entity in $entities) {
    # 1. Update Repository Interface
    $interfacePath = "$baseDir\ProductAPI.Application\Repositories\I${entity}Repository.cs"
    $interfaceContent = @"
using ProductAPI.Domain.Entities;
namespace ProductAPI.Application.Repositories;
public interface I${entity}Repository {
    Task<${entity}?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<${entity}>> GetAllAsync(CancellationToken cancellationToken = default);
    Task AddAsync(${entity} entity, CancellationToken cancellationToken = default);
    void Update(${entity} entity);
    void Delete(${entity} entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
"@
    Set-Content -Path $interfacePath -Value $interfaceContent

    # 2. Update Repository Implementation
    $repoPath = "$baseDir\ProductAPI.Infrastructure\Persistence\Repositories\${entity}Repository.cs"
    $repoContent = @"
using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Entities;
namespace ProductAPI.Infrastructure.Persistence.Repositories;
public class ${entity}Repository : I${entity}Repository {
    private readonly ApplicationDbContext _context;
    public ${entity}Repository(ApplicationDbContext context) { _context = context; }
    
    public async Task<${entity}?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) {
        return await _context.Set<${entity}>().FindAsync(new object[] { id }, cancellationToken);
    }
    public async Task<IEnumerable<${entity}>> GetAllAsync(CancellationToken cancellationToken = default) {
        return await _context.Set<${entity}>().ToListAsync(cancellationToken);
    }
    public async Task AddAsync(${entity} entity, CancellationToken cancellationToken = default) {
        await _context.Set<${entity}>().AddAsync(entity, cancellationToken);
    }
    public void Update(${entity} entity) {
        _context.Set<${entity}>().Update(entity);
    }
    public void Delete(${entity} entity) {
        _context.Set<${entity}>().Remove(entity);
    }
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) {
        return await _context.SaveChangesAsync(cancellationToken);
    }
}
"@
    Set-Content -Path $repoPath -Value $repoContent

    # 3. Create GetById Query
    mkdir -Force "$baseDir\ProductAPI.Application\Features\${entity}s\Queries\Get${entity}ById" | Out-Null
    
    $dtoProps = ""
    if ($entity -eq "Brand") { $dtoProps = "entity.Id, entity.Name" }
    elseif ($entity -eq "Category") { $dtoProps = "entity.Id, entity.Name, entity.Description" }
    elseif ($entity -eq "Review") { $dtoProps = "entity.Id, entity.ProductId, entity.UserName, entity.Comment, entity.Rating" }

    Set-Content -Path "$baseDir\ProductAPI.Application\Features\${entity}s\Queries\Get${entity}ById\Get${entity}ByIdQuery.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
namespace ProductAPI.Application.Features.${entity}s.Queries.Get${entity}ById;
public record Get${entity}ByIdQuery(Guid Id) : IRequest<${entity}Dto>;
"@

    Set-Content -Path "$baseDir\ProductAPI.Application\Features\${entity}s\Queries\Get${entity}ById\Get${entity}ByIdQueryHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.DTOs;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.${entity}s.Queries.Get${entity}ById;
public class Get${entity}ByIdQueryHandler : IRequestHandler<Get${entity}ByIdQuery, ${entity}Dto> {
    private readonly I${entity}Repository _repository;
    public Get${entity}ByIdQueryHandler(I${entity}Repository repository) { _repository = repository; }
    public async Task<${entity}Dto> Handle(Get${entity}ByIdQuery request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("${entity}", request.Id);
        return new ${entity}Dto($dtoProps);
    }
}
"@

    # 4. Create Update Command
    mkdir -Force "$baseDir\ProductAPI.Application\Features\${entity}s\Commands\Update${entity}" | Out-Null
    
    # We will just accept the same props as create for simplicity
    $cmdProps = ""
    $updateLogic = ""
    if ($entity -eq "Brand") { 
        $cmdProps = "Guid Id, string Name"
        $updateLogic = "entity.GetType().GetProperty(`"Name`").SetValue(entity, request.Name);"
    } elseif ($entity -eq "Category") { 
        $cmdProps = "Guid Id, string Name, string Description"
        $updateLogic = "entity.GetType().GetProperty(`"Name`").SetValue(entity, request.Name); entity.GetType().GetProperty(`"Description`").SetValue(entity, request.Description);"
    } elseif ($entity -eq "Review") { 
        $cmdProps = "Guid Id, string Comment, int Rating"
        $updateLogic = "entity.GetType().GetProperty(`"Comment`").SetValue(entity, request.Comment); entity.GetType().GetProperty(`"Rating`").SetValue(entity, request.Rating);"
    }

    Set-Content -Path "$baseDir\ProductAPI.Application\Features\${entity}s\Commands\Update${entity}\Update${entity}Command.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.${entity}s.Commands.Update${entity};
public record Update${entity}Command($cmdProps) : IRequest;
"@

    Set-Content -Path "$baseDir\ProductAPI.Application\Features\${entity}s\Commands\Update${entity}\Update${entity}CommandHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.${entity}s.Commands.Update${entity};
public class Update${entity}CommandHandler : IRequestHandler<Update${entity}Command> {
    private readonly I${entity}Repository _repository;
    public Update${entity}CommandHandler(I${entity}Repository repository) { _repository = repository; }
    public async Task Handle(Update${entity}Command request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("${entity}", request.Id);
        $updateLogic
        _repository.Update(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
"@

    # 5. Create Delete Command
    mkdir -Force "$baseDir\ProductAPI.Application\Features\${entity}s\Commands\Delete${entity}" | Out-Null

    Set-Content -Path "$baseDir\ProductAPI.Application\Features\${entity}s\Commands\Delete${entity}\Delete${entity}Command.cs" -Value @"
using MediatR;
namespace ProductAPI.Application.Features.${entity}s.Commands.Delete${entity};
public record Delete${entity}Command(Guid Id) : IRequest;
"@

    Set-Content -Path "$baseDir\ProductAPI.Application\Features\${entity}s\Commands\Delete${entity}\Delete${entity}CommandHandler.cs" -Value @"
using MediatR;
using ProductAPI.Application.Repositories;
using ProductAPI.Domain.Exceptions;
namespace ProductAPI.Application.Features.${entity}s.Commands.Delete${entity};
public class Delete${entity}CommandHandler : IRequestHandler<Delete${entity}Command> {
    private readonly I${entity}Repository _repository;
    public Delete${entity}CommandHandler(I${entity}Repository repository) { _repository = repository; }
    public async Task Handle(Delete${entity}Command request, CancellationToken cancellationToken) {
        var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (entity == null) throw new NotFoundException("${entity}", request.Id);
        _repository.Delete(entity);
        await _repository.SaveChangesAsync(cancellationToken);
    }
}
"@

    # 6. Update Controller
    $controllerPath = "$baseDir\ProductAPI.Api\Controllers\${entity}sController.cs"
    $controllerContent = @"
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.Features.${entity}s.Commands.Create${entity};
using ProductAPI.Application.Features.${entity}s.Commands.Update${entity};
using ProductAPI.Application.Features.${entity}s.Commands.Delete${entity};
using ProductAPI.Application.Features.${entity}s.Queries.Get${entity}s;
using ProductAPI.Application.Features.${entity}s.Queries.Get${entity}ById;

namespace ProductAPI.Api.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ${entity}sController : ControllerBase {
    private readonly IMediator _mediator;
    public ${entity}sController(IMediator mediator) { _mediator = mediator; }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Create${entity}Command command) {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = id }, new { Id = id });
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _mediator.Send(new Get${entity}sQuery()));
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id) => Ok(await _mediator.Send(new Get${entity}ByIdQuery(id)));
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Update${entity}Command command) {
        if (id != command.Id) return BadRequest("Id mismatch");
        await _mediator.Send(command);
        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id) {
        await _mediator.Send(new Delete${entity}Command(id));
        return NoContent();
    }
}
"@
    Set-Content -Path $controllerPath -Value $controllerContent
}
