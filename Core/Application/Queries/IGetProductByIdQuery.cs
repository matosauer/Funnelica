namespace Application.Queries;

public interface IGetProductByIdQuery
{
    Task<ProductResponse?> HandleAsync(Guid id);
}
