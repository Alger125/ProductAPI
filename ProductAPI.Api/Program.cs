using Microsoft.EntityFrameworkCore;
using ProductAPI.Application.Repositories;
using ProductAPI.Infrastructure.Persistence;
using ProductAPI.Infrastructure.Persistence.Repositories;
using ProductAPI.Application.Features.Products.Commands.CreateProduct;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 1. CONFIGURACIÓN DE BASE DE DATOS (Entity Framework Core)
// ============================================================
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// ============================================================
// 2. INYECCIÓN DE DEPENDENCIAS (MediatR y Repositorios)
// ============================================================
// Registramos el Repositorio de la capa Infrastructure
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

// Registramos MediatR escaneando el assembly de la capa Application
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// Agregamos soporte para Controladores
builder.Services.AddControllers();

// ============================================================
// 3. SWAGGER Y CONFIGURACIÓN BÁSICA
// ============================================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(options =>
    {
        options.SerializeAsV2 = true;
    });
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapeamos los controladores (nuestro ProductsController)
app.MapControllers();

app.Run();