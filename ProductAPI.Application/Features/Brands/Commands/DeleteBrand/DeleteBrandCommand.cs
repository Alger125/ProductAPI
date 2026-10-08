using MediatR;
namespace ProductAPI.Application.Features.Brands.Commands.DeleteBrand;
public record DeleteBrandCommand(Guid Id) : IRequest;
