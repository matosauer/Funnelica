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
    public class ProductsController : ControllerBase
    {
        private readonly ProductService service;

        public ProductsController(ProductService service)
        {
            this.service = service;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            return await service.GetAllProductsAsync();
        }

        // GET api/<ProductsController>/23e4567-e89b-12d3-a456-426614174000
        [HttpGet("{id}")]
        public async Task<Results<Ok<ProductDto>, NotFound>> Get(Guid id)
        {
            var product = await service.GetByIdAsync(id);
            return product != null ? TypedResults.Ok(product) : TypedResults.NotFound();
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<ProductDto> Post([FromBody] ProductDto dto)
        {
            var productDto = await service.CreateProductAsync(dto);
            return productDto;
        }

        // PUT api/<ProductsController>/23e4567-e89b-12d3-a456-426614174000
        [HttpPut("{id}")]
        public async Task Update(Guid id, [FromBody] ProductDto dto)
        {
            await service.UpdateProductAsync(dto);
        }

        // DELETE api/<ProductsController>/23e4567-e89b-12d3-a456-426614174000
        [HttpDelete("{id}")]
        public async Task Delete(Guid id)
        {
            await service.DeleteProductAsync(id);
        }
    }
}
