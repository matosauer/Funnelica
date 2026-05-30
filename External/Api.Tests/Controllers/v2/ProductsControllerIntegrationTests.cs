using System.Net;
using System.Net.Http.Json;
using Application.DTOs;
using Application.Services;
using Moq;

namespace Api.Tests.Controllers.v2;

public class ProductsControllerIntegrationTests : IClassFixture<ApiWebApplicationFactory>
{
    private readonly ApiWebApplicationFactory factory;

    public ProductsControllerIntegrationTests(ApiWebApplicationFactory factory)
    {
        this.factory = factory;
        factory.ProductServiceMock.Reset();
    }

    [Fact]
    public async Task Get_ReturnsOk_WithProducts()
    {
        var products = new List<ProductDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Integration Product",
                Price = 250,
                QuantityInStock = 3
            }
        };

        factory.ProductServiceMock
            .Setup(s => s.GetAllProductsAsync())
            .ReturnsAsync(products);

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v2/Products");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
        Assert.Single(body!);
        Assert.Equal("Integration Product", body![0].Name);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var id = Guid.NewGuid();
        factory.ProductServiceMock
            .Setup(s => s.GetByIdAsync(id))
            .ReturnsAsync((ProductDto?)null);

        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/v2/Products/{id}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Post_ReturnsCreated_WhenSuccessful()
    {
        var created = new ProductDto
        {
            Id = Guid.NewGuid(),
            Name = "New Product",
            Price = 99,
            QuantityInStock = 1
        };

        factory.ProductServiceMock
            .Setup(s => s.CreateProductAsync(It.IsAny<ProductDto>()))
            .ReturnsAsync(new ProductServiceResult
            {
                Success = true,
                ProductDto = created
            });

        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v2/Products", new ProductDto
        {
            Name = "New Product",
            Price = 99,
            QuantityInStock = 1
        });

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<ProductDto>();
        Assert.Equal(created.Id, body!.Id);
    }
}
