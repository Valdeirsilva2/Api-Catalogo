using APICatalogo.Context;
using APICatalogo.Dtos;
using APICatalogo.Model;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController(AppDbContext context, ICategoryRepository categoryRepository, ILogger<CategoryController> logger) : ControllerBase
{

    [HttpGet("products")]
    public ActionResult<IEnumerable<Category>> GetCategoriesWithProducts()
    {
        try
        {
            var categories = categoryRepository.GetCategoriesWithProducts();
            return Ok(categories);
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
            var categories = categoryRepository.GetCategories();
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

        var createdCategory = categoryRepository.CreateCategory(category);

        return new CreatedAtRouteResult("GetCategoryById", new { id = createdCategory.CategoryId }, createdCategory);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        try
        {
            var deletedCategory = categoryRepository.DeleteCategory(id);
            return Ok(deletedCategory);
        }
        catch (ArgumentNullException)
        {
            return NotFound("Category not found.");
        }
        catch (Exception e)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Erro ao deletar categoria \nError: " + e.Message);
        }
    }
}