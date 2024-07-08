using CSI.Unicard.Application.DTOs;
using CSI.Unicard.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CSI.Unicard.API.Controllers
{
    /// <summary>
    /// Controller for managing products.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="productService">The service for managing products.</param>
        /// <param name="logger">The logger instance.</param>
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, with a list of product DTOs.</returns>
        [HttpGet]
        [Route(nameof(GetAllProduct))]
        public async Task<IEnumerable<ProductDTO>> GetAllProduct()
        {
            _logger.LogInformation("Getting all products");
            var res = await _productService.GetAll();
            _logger.LogInformation("Products retrieved: {@Products}", res);
            return res;
        }

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>A task representing the asynchronous operation, with the product DTO.</returns>
        [HttpGet]
        [Route(nameof(GetById))]
        public async Task<ProductDTO> GetById(int id)
        {
            _logger.LogInformation("Getting product by id: {Id}", id);
            var res = await _productService.GetById(id);
            _logger.LogInformation("Product retrieved: {@Product}", res);
            return res;
        }

        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="product">The product to add.</param>
        /// <returns>A task representing the asynchronous operation, with an IActionResult containing the result.</returns>
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Add([FromBody] ProductDTO product)
        {
            _logger.LogInformation("Adding product: {@Product}", product);
            try
            {
                var result = await _productService.Add(product);
                _logger.LogInformation("Product added successfully with result: {Result}", result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding product.");
                return StatusCode(500, "Internal server error");
            }
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product to delete.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        [HttpDelete]
        [Route("[action]")]
        public async Task DeleteById(int id)
        {
            await _productService.DeleteById(id);
        }
    }
}
