using Domain.Entities;

namespace Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveAsync(CancellationToken cancellationToken = default);
        IGenericRepository<Product> ProductRepository { get; }
    }
}
