using Catalog.Api.Entities;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        #region Constructor's

        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository productRepository, ILogger<CatalogController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        #endregion

        #region Method's

        #region Get All

        [HttpGet]
        [ProducesResponseType(type: typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var product = await _productRepository
                .GetAllProductsAsync();

            return Ok(product);
        }

        #endregion

        #region Get Product By Id

        [HttpGet(template: "{id:length(24)}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(type: typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await _productRepository
                .GetByIdAsync(id: id);

            if (product == null)
            {
                _logger.LogError(message: $"Product with Id :{id} not found ");
                return NotFound();
            }

            return Ok(product);
        }

        #endregion

        #region Get By name

        [HttpGet(template: "[action]/{name}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(type: typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName(string name)
        {
            var product = await _productRepository
                .GetByNameAsync(name: name);

            if (product == null)
            {
                _logger.LogError(message: $"Product with name :{name} not found ");
                return NotFound();
            }

            return Ok(product);
        }

        #endregion

        #region Get By category

        [HttpGet(template: "[action]/{category}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(type: typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductsByCategory(string category)
        {
            var product = await _productRepository
                .GetByCategoryAsync(categoryName: category);

            if (product == null)
            {
                _logger.LogError(message: $"Product with category :{category} not found ");
                return NotFound();
            }

            return Ok(product);
        }

        #endregion

        #region Create

        [HttpPost]
        [ProducesResponseType(type: typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> CreateProduct([FromBody] Product product)
        {
            await _productRepository
               .CreateProductAsync(product: product);

            return CreatedAtRoute
            (
                routeName: "GetProductById",
                routeValues: new { id = product.Id },
                value: product
            );
        }

        #endregion

        #region Update

        [HttpPut]
        [ProducesResponseType(type: typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            var result = await _productRepository
               .UpdateProductAsync(product: product);

            return Ok(result);
        }

        #endregion

        #region Delete

        [HttpDelete(template: "{id:length(24)}", Name = "DeleteProduct")]
        [ProducesResponseType(type: typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var result = await _productRepository
               .DeleteProductAsync(id: id);

            return Ok(result);
        }

        #endregion

        #endregion
    }
}
