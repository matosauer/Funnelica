namespace Application.Queries;

public interface IGetProductByIdQuery
{
    Task<ProductTO?> HandleAsync(Guid id);
}
