using APICatalogo.Model;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(
    IProductRepository productRepository, // Injeção de dependência do repositório específico para produtos nao e necessaria,
                                          // mas é uma boa prática para manter a coesão e facilitar a manutenção do código.
    IRepository<Product> repository, 
    ILogger<ProductsController> logger,
    IRepository<Category> categoryRepository) : ControllerBase
{
    [HttpGet("products/{id:int}")]
    public ActionResult<IEnumerable<Product>> GetProductsByCategoryId(int id)
    {
        try
        {
            var products = productRepository.GetCategoriesWithProducts(id);
            logger.LogInformation("Products for category id {Id} retrieved successfully.", id);
            return Ok(products);
        }
        catch (Exception e)
        {
            logger.LogWarning("Error retrieving products for category id {Id}: {Message}", id, e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error retrieving products for category \nError: " + e.Message);
        }
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
    {
        try
        {
            var products =  repository.GetAll();
            logger.LogInformation("Products retrieved successfully.");
            return Ok(products);
        }
        catch (Exception e)
        {
            logger.LogWarning("Error retrieving products: {Message}", e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error retrieving products \nError: " + e.Message);
        }
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        try
        {
            var product = repository.Get(p => p.ProductId == id);
            return Ok(product);
        }
        catch (Exception e)
        {
            logger.LogWarning("Error retrieving product: {Message}", e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error retrieving product \nError: " + e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(Product? product)
    {
        try
        {
            if (product is null)
                return BadRequest("Product cannot be null.");

            // Validação: categoria existe?
            var category = categoryRepository.Get(c => c.CategoryId == product.CategoryId);
            if (category is null)
            {
                logger.LogWarning("Categoria com id {CategoryId} não existe ao tentar criar produto.", product.CategoryId);
                return BadRequest($"Categoria com id {product.CategoryId} não existe.");
            }

            product.RegistryDate = DateTime.Now;
            var item = productRepository.Create(product);
            logger.LogInformation("Product created successfully.");
                
            return Ok(item);
        }
        catch (Exception e)
        {
            logger.LogWarning("Error creating product: {Message}", e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error saving product \nError: " + e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Product product)
    {
        try
        {
            if (id != product.ProductId)
            {
                logger.LogWarning("Product with id {Id} not found for update", id);
                return NotFound("Product not found.");
            }
            var productUpdated = repository.Update(product);
            logger.LogInformation("Product with id {Id} updated successfully.", id);
            return Ok(productUpdated);
        }
        catch (Exception e)
        {
            logger.LogWarning("Error updating product: {Message}", e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error updating product \nError: " + e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var product = repository.Get(p=> p.ProductId == id);
            if (product is null)
            {
                logger.LogWarning("Product with id {Id} not found for deletion", id);
                return NotFound("Product not found.");
            }

            var deletedProduct = repository.Delete(product);
            logger.LogInformation("Product with id {Id} deleted successfully.", id);
            return Ok(deletedProduct);
        }
        catch (Exception e)
        {
            logger.LogWarning("Error deleting product: {Message}", e.Message);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error deleting product \nError: " + e.Message);
        }
    }
}