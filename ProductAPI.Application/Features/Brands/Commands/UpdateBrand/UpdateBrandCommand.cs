using MediatR;
namespace ProductAPI.Application.Features.Brands.Commands.UpdateBrand;
public record UpdateBrandCommand(Guid Id, string Name) : IRequest;
