using Api.Controllers.v2;
using Api.Tests.Helpers;
using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Api.Tests.Controllers.v2;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> serviceMock = new();
    private readonly ProductsController controller;

    public ProductsControllerTests()
    {
        controller = new ProductsController(serviceMock.Object);
    }

    private static ProductDto CreateDto(Guid? id = null) => new()
    {
        Id = id ?? Guid.NewGuid(),
        Name = "Test Product",
        Description = "Test description",
        Price = 100,
        QuantityInStock = 5
    };

    [Fact]
    public async Task Get_ReturnsAllProducts()
    {
        var products = new List<ProductDto> { CreateDto(), CreateDto() };
        serviceMock.Setup(s => s.GetAllProductsAsync()).ReturnsAsync(products);

        var result = await controller.Get();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returned = Assert.IsAssignableFrom<IEnumerable<ProductDto>>(okResult.Value);
        Assert.Equal(2, returned.Count());
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenProductExists()
    {
        var id = Guid.NewGuid();
        var product = CreateDto(id);
        serviceMock.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync(product);

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Get(id));

        Assert.Equal(StatusCodes.Status200OK, snapshot.StatusCode);
        var returned = HttpResultTestHelper.DeserializeBody<ProductDto>(snapshot);
        Assert.Equal(id, returned!.Id);
    }

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenProductDoesNotExist()
    {
        var id = Guid.NewGuid();
        serviceMock.Setup(s => s.GetProductByIdAsync(id)).ReturnsAsync((ProductDto?)null);

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Get(id));

        Assert.Equal(StatusCodes.Status404NotFound, snapshot.StatusCode);
    }

    [Fact]
    public async Task Post_CallsService_WhenSuccessful()
    {
        var dto = CreateDto();
        var created = CreateDto(Guid.NewGuid());
        serviceMock.Setup(s => s.CreateProductAsync(dto)).ReturnsAsync(new ProductServiceResult
        {
            Success = true,
            ProductDto = created
        });

        var result = await controller.Post(dto);

        serviceMock.Verify(s => s.CreateProductAsync(dto), Times.Once);
        Assert.IsAssignableFrom<IResult>(result);
    }

    [Theory]
    [InlineData(FailureType.Validation)]
    [InlineData(FailureType.BusinessRule)]
    [InlineData(FailureType.Unknown)]
    public async Task Post_ReturnsBadRequest_WhenServiceFailsWithNonNotFoundFailure(FailureType failureType)
    {
        var dto = CreateDto();
        serviceMock.Setup(s => s.CreateProductAsync(dto)).ReturnsAsync(new ProductServiceResult
        {
            Success = false,
            FailureType = failureType
        });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Post(dto));

        Assert.Equal(StatusCodes.Status400BadRequest, snapshot.StatusCode);
    }

    [Fact]
    public async Task Post_ReturnsNotFound_WhenServiceFailsWithNotFound()
    {
        var dto = CreateDto();
        serviceMock.Setup(s => s.CreateProductAsync(dto)).ReturnsAsync(new ProductServiceResult
        {
            Success = false,
            FailureType = FailureType.NotFound
        });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Post(dto));

        Assert.Equal(StatusCodes.Status404NotFound, snapshot.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenDtoIsNull()
    {
        var id = Guid.NewGuid();

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Update(id, null!));

        Assert.Equal(StatusCodes.Status400BadRequest, snapshot.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenDtoIdIsEmpty()
    {
        var id = Guid.NewGuid();
        var dto = CreateDto(Guid.Empty);

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Update(id, dto));

        Assert.Equal(StatusCodes.Status400BadRequest, snapshot.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsBadRequest_WhenRouteIdDoesNotMatchDtoId()
    {
        var dto = CreateDto(Guid.NewGuid());

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Update(Guid.NewGuid(), dto));

        Assert.Equal(StatusCodes.Status400BadRequest, snapshot.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        var dto = CreateDto(id);
        serviceMock.Setup(s => s.UpdateProductAsync(dto)).ReturnsAsync(new ProductServiceResult { Success = true });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Update(id, dto));

        Assert.Equal(StatusCodes.Status204NoContent, snapshot.StatusCode);
    }

    [Fact]
    public async Task Update_ReturnsNotFound_WhenServiceFailsWithNotFound()
    {
        var id = Guid.NewGuid();
        var dto = CreateDto(id);
        serviceMock.Setup(s => s.UpdateProductAsync(dto)).ReturnsAsync(new ProductServiceResult
        {
            Success = false,
            FailureType = FailureType.NotFound
        });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Update(id, dto));

        Assert.Equal(StatusCodes.Status404NotFound, snapshot.StatusCode);
    }

    [Theory]
    [InlineData(FailureType.Validation)]
    [InlineData(FailureType.BusinessRule)]
    [InlineData(FailureType.Unknown)]
    public async Task Update_ReturnsBadRequest_WhenServiceFailsWithNonNotFoundFailure(FailureType failureType)
    {
        var id = Guid.NewGuid();
        var dto = CreateDto(id);
        serviceMock.Setup(s => s.UpdateProductAsync(dto)).ReturnsAsync(new ProductServiceResult
        {
            Success = false,
            FailureType = failureType
        });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Update(id, dto));

        Assert.Equal(StatusCodes.Status400BadRequest, snapshot.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNoContent_WhenSuccessful()
    {
        var id = Guid.NewGuid();
        serviceMock.Setup(s => s.DeleteProductAsync(id)).ReturnsAsync(new ProductServiceResult { Success = true });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Delete(id));

        Assert.Equal(StatusCodes.Status204NoContent, snapshot.StatusCode);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenServiceFailsWithNotFound()
    {
        var id = Guid.NewGuid();
        serviceMock.Setup(s => s.DeleteProductAsync(id)).ReturnsAsync(new ProductServiceResult
        {
            Success = false,
            FailureType = FailureType.NotFound
        });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Delete(id));

        Assert.Equal(StatusCodes.Status404NotFound, snapshot.StatusCode);
    }

    [Theory]
    [InlineData(FailureType.Validation)]
    [InlineData(FailureType.BusinessRule)]
    [InlineData(FailureType.Unknown)]
    public async Task Delete_ReturnsBadRequest_WhenServiceFailsWithNonNotFoundFailure(FailureType failureType)
    {
        var id = Guid.NewGuid();
        serviceMock.Setup(s => s.DeleteProductAsync(id)).ReturnsAsync(new ProductServiceResult
        {
            Success = false,
            FailureType = failureType
        });

        var snapshot = await HttpResultTestHelper.InvokeAsync(await controller.Delete(id));

        Assert.Equal(StatusCodes.Status400BadRequest, snapshot.StatusCode);
    }
}
