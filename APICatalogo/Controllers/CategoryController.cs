using APICatalogo.Context;
using APICatalogo.Dtos;
using APICatalogo.Model;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController(AppDbContext context, ICategoryRepository categoryRepository, ILogger logger) : ControllerBase
{

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        try
        {
            return context.Categories.Include(c => c.Products).ToList();
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
            var categories = context.Categories.AsNoTracking().ToList();
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
        var category = categoryRepository.GetCategory(id);

        if (category is null)
        {
            logger.LogInformation("Category with id {Id} not found.", id);
            return NotFound($"Category with id= {id} not found.");

        }
        return Ok(category);

    }

    [HttpPut("{id}")]
    public ActionResult<Category> Put(int id, Category category)
    {
        throw new NotImplementedException();
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

        context.Categories.Add(category);
        context.SaveChanges(); // Save changes to the database

        return new CreatedAtRouteResult("GetCategoryById", new { id = category.CategoryId }, category);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);

        if (category is null)
        {
            return NotFound("Category not found.");
        }

        context.Categories.Remove(category);
        context.SaveChanges();

        return Ok(category);
    }
}