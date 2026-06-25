using Application.DTOs;
using Application.Mappers;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class ProductService : IProductService
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

    public async Task<ProductDto?> GetProductByIdAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        return (product != null) ? Mapper.Map<ProductDto>(product) : null;
    }

    public async Task<ProductServiceResult> CreateProductAsync(ProductDto dto)
    {
        dto.Id = Guid.Empty;

        var product = Mapper.Map<Product>(dto);

        await unitOfWork.ProductRepository.AddAsync(product);
        await unitOfWork.SaveAsync();

        var productDto = Mapper.Map<ProductDto>(product);
        return new ProductServiceResult { Success = true, Message = "Product created successfully.", ProductDto = productDto };
    }

    public async Task<ProductServiceResult> UpdateProductAsync(ProductDto dto)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(dto.Id);
        if (product == null)
        {
            return new ProductServiceResult { Success = false, Message = $"Product with id {dto.Id} not found.", FailureType = FailureType.NotFound };
        }

        product.Name = dto.Name;
        product.Description = dto.Description;
        product.PictureUrl = dto.PictureUrl;
        product.Price = dto.Price;
        product.QuantityInStock = dto.QuantityInStock;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();

        return new ProductServiceResult { Success = true, Message = "Product updated successfully." };
    }

    public async Task<ProductServiceResult> DeleteProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ProductServiceResult { Success = false, Message = $"Product with id {id} not found.", FailureType = FailureType.NotFound };
        }

        await unitOfWork.ProductRepository.DeleteAsync(product);
        await unitOfWork.SaveAsync();

        return new ProductServiceResult { Success = true, Message = "Product deleted successfully." };
    }

    public async Task<ProductServiceResult> PublishProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ProductServiceResult { Success = false, Message = $"Product with id {id} not found.", FailureType = FailureType.NotFound };
        }

        product.PublishedOnUtc = DateTime.UtcNow;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();

        return new ProductServiceResult { Success = true, Message = "Product published successfully." };
    }

    public async Task<ProductServiceResult> UnpublishProductAsync(Guid id)
    {
        var product = await unitOfWork.ProductRepository.GetByIdAsync(id);
        if (product == null)
        {
            return new ProductServiceResult { Success = false, Message = $"Product with id {id} not found.", FailureType = FailureType.NotFound };
        }

        product.PublishedOnUtc = null;

        await unitOfWork.ProductRepository.UpdateAsync(product);
        await unitOfWork.SaveAsync();

        return new ProductServiceResult { Success = true, Message = "Product unpublished successfully." };
    }
}
