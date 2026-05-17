using Domain.Entities;

namespace Domain.Repositories;

public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task<Guid> InsertAsync(Product product);
    Task UpdateAsync(Product product);
}
