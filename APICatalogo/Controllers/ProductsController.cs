using APICatalogo.Context;
using APICatalogo.Dtos;
using APICatalogo.Model;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
    {
        try
        {
            var products =  productRepository.GetAllProducts();
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
            var product = productRepository.GetProductById(id);
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

            product.RegistryDate = DateTime.Now;
            var item = productRepository.CreateProduct(product);
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
    public ActionResult Put(int id, ProductUpdateDto productDto)
    {
        try
        {
            var existingProduct = productRepository.GetProductById(id);
            if (existingProduct is null)
            {
                logger.LogWarning("Product with id {Id} not found for update", id);
                return NotFound("Product not found.");
            }

            // Atualiza apenas os campos permitidos
            existingProduct.Name = productDto.Name;
            existingProduct.Description = productDto.Description;
            existingProduct.Value = productDto.Value;
            existingProduct.Stock = productDto.Stock;
            existingProduct.ImageUrl = productDto.ImageUrl;
            existingProduct.CategoryId = productDto.CategoryId;
            existingProduct.RegistryDate = productDto.RegistryDate;

            productRepository.UpdateProduct(existingProduct);
            logger.LogInformation("Product with id {Id} updated successfully.", id);
            return NoContent();
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
            var product = productRepository.GetProductById(id);
            if (product is null)
            {
                logger.LogWarning("Product with id {Id} not found for deletion", id);
                return NotFound("Product not found.");
            }

            productRepository.DeleteProduct(id);
            logger.LogInformation("Product with id {Id} deleted successfully.", id);
            return Ok(product);
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