using Application.DTOs;
using Application.Services;
using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.v2
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("2.0")]
    [Produces("application/json")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService service;

        public ProductsController(ProductService service)
        {
            this.service = service;
        }

        // GET: api/v2/<ProductsController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ProductDto>>> Get()
        {
            var products = await service.GetAllProductsAsync();
            return Ok(products);
        }

        // GET api/v2/<ProductsController>/<id>
        [HttpGet("{id}")]
        public async Task<Results<Ok<ProductDto>, NotFound>> Get(Guid id)
        {
            var product = await service.GetByIdAsync(id);
            return product != null ? TypedResults.Ok(product) : TypedResults.NotFound();
        }

        // POST api/v2/<ProductsController>
        [HttpPost]
        public async Task<Results<CreatedAtRoute<ProductDto>, NotFound, BadRequest>> Post([FromBody] ProductDto dto)
        {
            var result = await service.CreateProductAsync(dto);
            return MapCreatedToActionResult(result);
        }

        // PUT api/v2/<ProductsController>/<id>
        [HttpPut("{id}")]
        public async Task<Results<NoContent, NotFound, BadRequest>> Update(Guid id, [FromBody] ProductDto dto)
        {
            // Optionally validate id matches dto.Id
            if (dto == null || dto.Id == Guid.Empty || dto.Id != id)
            {
                return TypedResults.BadRequest();
            }

            var result = await service.UpdateProductAsync(dto);
            return MapServiceResultToActionResult(result);
        }

        // DELETE api/v2/<ProductsController>/<id>
        [HttpDelete("{id}")]
        public async Task<Results<NoContent, NotFound, BadRequest>> Delete(Guid id)
        {
            var result = await service.DeleteProductAsync(id);
            return MapServiceResultToActionResult(result);
        }

        private static Results<CreatedAtRoute<ProductDto>, NotFound, BadRequest> MapCreatedToActionResult(ProductServiceResult result)
        {
            if (result.Success)
            {
                return TypedResults.CreatedAtRoute(
                   result.ProductDto,
                   routeValues: new { id = result.ProductDto!.Id });
            }
            if (result.FailureType == FailureType.Validation || result.FailureType == FailureType.BusinessRule)
            {
                return TypedResults.BadRequest();
            }
            if (result.FailureType == FailureType.NotFound)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.BadRequest();
        }

        private static Results<NoContent, NotFound, BadRequest> MapServiceResultToActionResult(ProductServiceResult result)
        {
            if (result.Success)
            {
                return TypedResults.NoContent();
            }
            if (result.FailureType == FailureType.Validation || result.FailureType == FailureType.BusinessRule)
            {
                return TypedResults.BadRequest();
            }
            if (result.FailureType == FailureType.NotFound)
            {
                return TypedResults.NotFound();
            }
            return TypedResults.BadRequest();
        }
    }
}
