using Catalog.API.Entities;
using Catalog.API.Repositories;
using EventBus.Messages.Events;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[Controller]")]
    
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger, IPublishEndpoint publishEndpoint)
        {
            _productRepository = productRepository;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }






        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _productRepository.GetProducts();
            return Ok(products);
        }





        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var products = await _productRepository.GetProduct(id);
            if (products == null)
            {
                _logger.LogError($"\"Product wih id: {id}, not found.");
                return NotFound();
            }
            _logger.LogInformation($"Product wih id: {id}, was found.");
            return Ok(products);
        }




        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductByCategory(string category)
        {
            var product = await _productRepository.GetProductByCategory(category);
            if (product == null)
            {
                _logger.LogError($"Product wih category: {category}, not found.");
                return NotFound();
            }
            return Ok(product);
        }





        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await _productRepository.CreateProduct(product);
            return CreatedAtAction(actionName: "GetProduct", new { id = product.Id }, product);
            // it means when we create a new product then we go to get product with that id so we can sure we create it right ?
        }





        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            var oldproduct = await _productRepository.GetProduct(product.Id);

            if (product.Name != oldproduct.Name)
            {
                ProductUpdateEvent eventMessage = new ProductUpdateEvent()
                {
                    ProductName = oldproduct.Name,
                    NewProductName = product.Name
                };

                await _publishEndpoint.Publish(eventMessage);
            }

            var ret = await _productRepository.UpdateProduct(product);

            return Ok(ret);
            
            
        }






        [HttpDelete("{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            return Ok(await _productRepository.DeleteProduct(id));
        }

    }
}
