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
        public async Task<Results<CreatedAtRoute<ProductDto>, BadRequest>> Post([FromBody] ProductDto dto)
        {
            var actResult = await service.CreateProductAsync(dto);

            if (!actResult.Success)
            {
                // Optionally log validation errors here
                return TypedResults.BadRequest();
            }

            return TypedResults.CreatedAtRoute(
                        routeName: nameof(Get),
                        routeValues: new { id = actResult?.Product?.Id },
                        value: actResult?.Product
                    );
        }

        // PUT api/v2/<ProductsController>/<id>
        [HttpPut("{id}")]
        public async Task<Results<NoContent, BadRequest, NotFound>> Update(Guid id, [FromBody] ProductDto dto)
        {
            // Optionally validate id matches dto.Id
            if (dto == null || dto.Id == Guid.Empty || dto.Id != id)
            {
                return TypedResults.BadRequest();
            }

            ActResult actResult = await service.UpdateProductAsync(dto);
            return actResult.Success ? TypedResults.NoContent() : TypedResults.NotFound();
        }

        // DELETE api/v2/<ProductsController>/<id>
        [HttpDelete("{id}")]
        public async Task<Results<NoContent, NotFound>> Delete(Guid id)
        {
            var actResult = await service.DeleteProductAsync(id);
            return actResult.Success ? TypedResults.NoContent() : TypedResults.NotFound();
        }
    }
}
