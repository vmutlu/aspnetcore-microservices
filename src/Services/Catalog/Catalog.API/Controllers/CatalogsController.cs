using Catalog.API.Entities;
using Catalog.API.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogsController> _logger;

        public CatalogsController(IProductRepository productRepository, ILogger<CatalogsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts().ConfigureAwait(false);

            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = nameof(GetProduct))]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProduct(string id)
        {
            var product = await _productRepository.GetProduct(id).ConfigureAwait(false);

            if (product is null)
            {
                _logger.LogError($"Product with id: {id} not found");
                return NotFound();
            }

            return Ok(product);
        }

        [HttpGet(nameof(GetProductByCategory))]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var products = await _productRepository.GetProductByCategory(category).ConfigureAwait(false);

            return Ok(products);
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            await _productRepository.AddProduct(product).ConfigureAwait(false);

            return CreatedAtRoute(nameof(GetProduct), new { id = product.Id.ToString() }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _productRepository.UpdateProduct(product).ConfigureAwait(false));
        }

        [HttpDelete("{id:length(24)}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id).ConfigureAwait(false));
        }
    }
}
