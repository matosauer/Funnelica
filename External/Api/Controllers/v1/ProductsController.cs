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

        public ProductsController(ILogger<ProductsController> logger)
        {
            this.logger = logger;
        }

        // GET: api/v1/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<ProductDto>> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/v1/<ProductsController>/<id>
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

        // PUT api/v1/<ProductsController>/<id>
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] string value)
        {
        }

        // DELETE api/v1/<ProductsController>/<id>
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
