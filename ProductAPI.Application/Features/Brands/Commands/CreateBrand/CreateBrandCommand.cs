using MediatR;
namespace ProductAPI.Application.Features.Brands.Commands.CreateBrand;
public record CreateBrandCommand(string Name, string Country) : IRequest<Guid>;
