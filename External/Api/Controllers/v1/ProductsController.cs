using Application.DTOs;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> logger;

        public ProductsController(/*ProductService service*/ ILogger<ProductsController> logger)
        {
            this.logger = logger;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/<ProductsController>/23e4567-e89b-12d3-a456-426614174000
        [HttpGet("{id}")]
        [ProducesResponseType<ProductDto>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductDto>> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/23e4567-e89b-12d3-a456-426614174000
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/23e4567-e89b-12d3-a456-426614174000
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
