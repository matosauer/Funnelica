using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class ProductService
{
    private readonly IUnitOfWork unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<List<ProductDto>> GetAllProductsAsync()
    {
        var products = await unitOfWork.ProductRepository.GetAllAsync();
        return products.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Price = p.Price
        }).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }

        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price
        };
    }

    public async Task<ProductDto> CreateProductAsync(ProductDto dto)
    {
        var product = new Product
        {
            Id = Guid.Empty,
            Name = dto.Name,
            Description = dto.Description,
            PictureUrl = dto.PictureUrl,
            Price = dto.Price,
            QuantityInStock = dto.QuantityInStock,
            CreatedOnUtc = DateTime.UtcNow
        };

        await unitOfWork.ProductRepository.AddAsync(product);
        await unitOfWork.SaveAsync();

        ProductDto productDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            PictureUrl = product.PictureUrl,
            Price = product.Price,
            QuantityInStock = product.QuantityInStock
        };

        return productDto;
    }

    public async Task UpdateProductAsync(ProductDto dto)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(dto.Id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {dto.Id} not found.");
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.PictureUrl = dto.PictureUrl;
        product.Price = dto.Price;
        product.QuantityInStock = dto.QuantityInStock;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();
    }

    public async Task DeleteProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }
        await unitOfWork.ProductRepository.DeleteAsync(product);
        await unitOfWork.SaveAsync();
    }

    public async Task PublishProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }

        product.PublishedOnUtc = DateTime.UtcNow;
        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();
    }

    public async Task UnpublishProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product is null)
        {
            throw new InvalidOperationException($"Product with id {id} not found.");
        }

        product.PublishedOnUtc = null;
        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();
    }
}
