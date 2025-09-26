using APICatalogo.Context;
using APICatalogo.Dtos;
using APICatalogo.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoryController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        try
        {
            return _context.Categories.Include(c => c.Products).ToList();
        }
        catch (Exception e)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                "Erro ao obter categorias e seus produtos \nError: " + e.Message);
        }
    }

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        try
        {
             var categories = _context.Categories.AsNoTracking().ToList();
             return Ok(categories);
        }
        catch (Exception e)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError, 
                "Ocorreu um erro ao obter categoria \nError: " + e.Message);
        }
    }

    [HttpGet("{id}", Name = "GetCategoryById")]
    public ActionResult<Category> GetCategoryById(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);
        if (category == null)
        {
            return NotFound("Category not found.");
        }

        return Ok(category);
    }

    [HttpPut("{id}")]
    public ActionResult<Category> Put(int id, Category category)
    {
        if (id != category.CategoryId)
        {
            return BadRequest("Category ID mismatch.");
        }

        _context.Entry(category).State = EntityState.Modified;
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPost]
    public ActionResult Post(CreateCategoryDto? dto)
    {
        if (dto == null)
            return BadRequest("Category cannot be null.");

        var category = new Category()
        {
            Name = dto.Name,
            ImageUrl = dto.ImageUrl
        };

        _context.Categories.Add(category);
        _context.SaveChanges(); // Save changes to the database

        return new CreatedAtRouteResult("GetCategoryById", new { id = category.CategoryId }, category);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var category = _context.Categories.FirstOrDefault(c => c.CategoryId == id);

        if (category is null)
        {
            return NotFound("Category not found.");
        }

        _context.Categories.Remove(category);
        _context.SaveChanges();

        return Ok(category);
    }
}