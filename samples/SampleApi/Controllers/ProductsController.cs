using GenericRepositoryEF.Core.Interfaces;
using GenericRepositoryEF.Core.Models;
using GenericRepositoryEF.Extensions.Repositories;
using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Specifications;

namespace SampleApi.Controllers
{
    /// <summary>
    /// API controller for managing products.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ProductsController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="productRepository">The product repository.</param>
        /// <param name="categoryRepository">The category repository.</param>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="logger">The logger.</param>
        public ProductsController(
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IUnitOfWork unitOfWork,
            ILogger<ProductsController> logger)
        {
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all products.
        /// </summary>
        /// <returns>The list of products.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts()
        {
            _logger.LogInformation("Getting all products");
            var products = await _productRepository.ListAsync(ProductSpecifications.WithRelationships());
            return Ok(products);
        }

        /// <summary>
        /// Gets a product by identifier.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>The product.</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(int id)
        {
            _logger.LogInformation("Getting product with id {ProductId}", id);
            var product = await _productRepository.GetByIdAsync(id);
            
            if (product == null)
            {
                return NotFound();
            }
            
            // Get the product with relationships
            var spec = new ProductSpecification(p => p.Id == id);
            spec.AddInclude(p => p.Category);
            spec.AddInclude(p => p.Supplier);
            
            var productWithRelationships = await _productRepository.GetSingleBySpecificationAsync(spec);
            
            return Ok(productWithRelationships ?? product);
        }

        /// <summary>
        /// Gets products by category.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <returns>The list of products.</returns>
        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            _logger.LogInformation("Getting products for category {CategoryId}", categoryId);
            
            var categoryExists = await _categoryRepository.ExistsAsync(c => c.Id == categoryId);
            if (!categoryExists)
            {
                return NotFound($"Category with ID {categoryId} not found");
            }
            
            var spec = ProductSpecifications.ByCategory(categoryId)
                .Compose(ProductSpecifications.WithRelationships());
                
            var products = await _productRepository.ListAsync(spec);
            return Ok(products);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="product">The product to create.</param>
        /// <returns>The created product.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            if (product == null)
            {
                return BadRequest();
            }

            _logger.LogInformation("Creating new product");
            
            // Reset ID to ensure proper creation
            product.Id = 0;
            
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <param name="product">The updated product.</param>
        /// <returns>No content if successful.</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (product == null || id != product.Id)
            {
                return BadRequest();
            }

            var existingProduct = await _productRepository.GetByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Updating product with id {ProductId}", id);
            
            await _productRepository.UpdateAsync(product);
            await _unitOfWork.SaveChangesAsync();
            
            return NoContent();
        }

        /// <summary>
        /// Deletes a product.
        /// </summary>
        /// <param name="id">The product identifier.</param>
        /// <returns>No content if successful.</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _logger.LogInformation("Deleting product with id {ProductId}", id);
            
            await _productRepository.DeleteAsync(product);
            await _unitOfWork.SaveChangesAsync();
            
            return NoContent();
        }

        /// <summary>
        /// Gets products by search term.
        /// </summary>
        /// <param name="searchTerm">The search term.</param>
        /// <returns>The list of products.</returns>
        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResult<Product>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SearchProducts(
            [FromQuery] string searchTerm,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            _logger.LogInformation("Searching products with term {SearchTerm}", searchTerm);
            
            var spec = ProductSpecifications.ByNameSearch(searchTerm)
                .Compose(ProductSpecifications.WithRelationships());
                
            var result = await _productRepository.ListPagedAsync(
                specification: spec,
                pageNumber: pageNumber,
                pageSize: pageSize);
                
            return Ok(result);
        }
    }
}