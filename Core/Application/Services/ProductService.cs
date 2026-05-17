using Domain.Repositories;

namespace Application.Services;

public class ProductService
{
    private readonly IProductRepository repository;

    public ProductService(IProductRepository repository)
    {
        this.repository = repository;
    }

    //public async Task<List<Product>> GetAllProductsAsync()
    //{
    //    return await repository.GetAllAsync();
    //}

    public async Task<Guid> CreateProductAsync(string name, string description, string pictureUrl, long price, int quantityInStock)
    {
        var product = new Domain.Entities.Product
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            PictureUrl = pictureUrl,
            Price = price,
            QuantityInStock = quantityInStock,
            CreatedOnUtc = DateTime.UtcNow
        };
        return await repository.InsertAsync(product);
    }

    public async Task UpdateProductAsync(Guid id, string name, string description, string pictureUrl, long price, int quantityInStock)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }
        product.Name = name;
        product.Description = description;
        product.PictureUrl = pictureUrl;
        product.Price = price;
        product.QuantityInStock = quantityInStock;
        await repository.UpdateAsync(product);
    }

    public async Task PublishProductAsync(Guid id)
    {
        var product = await repository.GetByIdAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }
        product.PublishedOnUtc = DateTime.UtcNow;
        await repository.UpdateAsync(product);
    }

}
