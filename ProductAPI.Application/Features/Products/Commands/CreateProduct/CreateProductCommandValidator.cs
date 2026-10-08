using FluentValidation;

namespace ProductAPI.Application.Features.Products.Commands.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre del producto es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre no puede exceder los 100 caracteres.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

        RuleFor(p => p.CategoryId)
            .NotEmpty().WithMessage("Se requiere un ID de categoría.");

        RuleFor(p => p.BrandId)
            .NotEmpty().WithMessage("Se requiere un ID de marca.");
    }
}
