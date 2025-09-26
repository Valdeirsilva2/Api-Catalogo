using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class ProductsController(AppDbContext context) : ControllerBase
{
    private readonly AppDbContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetAsync()
    {
         return Ok(await _context.Products.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}", Name = "GetProductById")]
    public async Task<ActionResult<Product>> GetByIdAsync(int id)
    {
        try
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            return Ok(product);
        }
        catch (Exception e)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error retrieving product \nError: " + e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(Product product)
    {
        try
        {
            if (product is null)
                return BadRequest("Product cannot be null.");

            product.RegistryDate = DateTime.Now; // <- Definindo a data de registro do produto

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // <- função assíncrona para salvar as alterações no banco de dados

            return new CreatedAtRouteResult("GetProductById", new { id = product.ProductId }, product);
        }
        catch (Exception e)
        {
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
                return BadRequest("Product ID mismatch.");

            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();

            return NoContent();
        }
        catch (Exception e)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Error updating product \nError: " + e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public ActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
        if (product is null)
        {
            return NotFound("Product not found.");
        }

        _context.Products.Remove(product);
        _context.SaveChanges();

        return Ok(product);
    }
}