using Application.DTOs;

namespace Application.Queries;

public interface IGetProductByIdQuery
{
    Task<ProductDto?> HandleAsync(Guid id);
}
