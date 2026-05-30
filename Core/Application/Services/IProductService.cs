using Application.DTOs;

namespace Application.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllProductsAsync();
    Task<ProductDto?> GetByIdAsync(Guid id);
    Task<ProductServiceResult> CreateProductAsync(ProductDto dto);
    Task<ProductServiceResult> UpdateProductAsync(ProductDto dto);
    Task<ProductServiceResult> DeleteProductAsync(Guid id);
    Task<ProductServiceResult> PublishProductAsync(Guid id);
    Task<ProductServiceResult> UnpublishProductAsync(Guid id);
}
