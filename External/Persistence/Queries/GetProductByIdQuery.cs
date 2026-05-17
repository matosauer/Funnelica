using Application.Queries;
using Microsoft.EntityFrameworkCore;
using Persistence.Database;

namespace Persistence.Queries
{
    public sealed class GetProductByIdQuery : IGetProductByIdQuery
    {
        private readonly ApplicationDbContext context;

        public GetProductByIdQuery(ApplicationDbContext context)
        {
            this.context = context;
        }
        public async Task<ProductResponse?> HandleAsync(Guid id)
        {
            var product = await context
                .Products
                .AsNoTracking()
                .Select(p => new ProductResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price
                })
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }
    }
}
