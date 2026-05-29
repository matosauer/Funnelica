using Application.DTOs;
using Application.Mappers;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class ActResult
{
    public bool Success { get; set; }
    public string? Message { get; set; } = null;
    public ProductDto? Product { get; set; } = null;
}

public class ProductService
{
    private readonly IUnitOfWork unitOfWork;

    public ProductService(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        var products = await unitOfWork.ProductRepository.GetAllAsync();
        return Mapper.Map<IEnumerable<ProductDto>>(products);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        return (product != null) ? Mapper.Map<ProductDto>(product) : null;
    }

    public async Task<ActResult> CreateProductAsync(ProductDto dto)
    {
        var product = Mapper.Map<Product>(dto);

        await unitOfWork.ProductRepository.AddAsync(product);
        await unitOfWork.SaveAsync();

        var productDto = Mapper.Map<ProductDto>(product);
        return new ActResult { Success = true, Message = "Product created successfully.", Product = productDto };
    }

    public async Task<ActResult> UpdateProductAsync(ProductDto dto)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(dto.Id);
        if (product == null)
        {
            return new ActResult { Success = false, Message = $"Product with id {dto.Id} not found." };
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.PictureUrl = dto.PictureUrl;
        product.Price = dto.Price;
        product.QuantityInStock = dto.QuantityInStock;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();

        return new ActResult { Success = true, Message = "Product updated successfully." };
    }

    public async Task<ActResult> DeleteProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ActResult { Success = false, Message = $"Product with id {id} not found." };
        }

        await unitOfWork.ProductRepository.DeleteAsync(product);
        await unitOfWork.SaveAsync();

        return new ActResult { Success = true, Message = "Product deleted successfully." };
    }

    public async Task<ActResult> PublishProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ActResult { Success = false, Message = $"Product with id {id} not found." };
        }

        product.PublishedOnUtc = DateTime.UtcNow;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();

        return new ActResult { Success = true, Message = "Product published successfully." };
    }

    public async Task<ActResult> UnpublishProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ActResult { Success = false, Message = $"Product with id {id} not found." };
        }

        product.PublishedOnUtc = null;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();

        return new ActResult { Success = true, Message = "Product unpublished successfully." };
    }
}
