using Application.Queries;
using Asp.Versioning;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers.v1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class ProductsController : ControllerBase
    {
        //private readonly ProductService service;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(/*ProductService service*/ ILogger<ProductsController> logger)
        {
            this.logger = logger;
            // this.service = service;
        }

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IEnumerable<ProductTO>> Get()
        {
            throw new NotImplementedException();
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        [ProducesResponseType<ProductTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductTO>> Get(Guid id, IGetProductByIdQuery handler)
        {
            var result = await handler.HandleAsync(id);
            return result != null ? Ok(result) : NotFound();
        }

        // POST api/<ProductsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public void Put(Guid id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public void Delete(Guid id)
        {
        }
    }
}
