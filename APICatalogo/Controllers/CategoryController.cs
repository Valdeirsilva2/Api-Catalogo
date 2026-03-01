using APICatalogo.Model;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController(
    ICategoryRepository categoryRepository,
    IRepository<Category> repository,
    ILogger<CategoryController> logger) : ControllerBase
{

    [HttpGet]
    public ActionResult<IEnumerable<Category>> Get()
    {
        try
        {
            var categories = repository.GetAll();
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
        var categories = repository.Get(c => c.CategoryId == id);

        if (categories is null)
        {
            logger.LogInformation("Category with id {Id} not found.", id);
            return NotFound($"Category with id= {id} not found.");

        }
        return Ok(categories);

    }

    [HttpPut("{id}")]
    public ActionResult<Category> Put(int id, Category category)
    {
        if (id != category.CategoryId)
            return BadRequest("Category ID mismatch.");

        try
        {
            categoryRepository.Update(category);
            return Ok(category);
        }
        catch (DbUpdateConcurrencyException)
        {
            if (repository.Get(c => c.CategoryId == id) == null)
                return NotFound($"Category with id= {id} not found.");
            else
                throw;
        }
        catch (Exception e)
        {
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                "Erro ao atualizar categoria \nError: " + e.Message);
        }
    }

    [HttpPost]
    public ActionResult Post(Category category)
    {
        if (category == null)
            return BadRequest("Category cannot be null.");

        try
        {
            var createdCategory = repository.Create(category);
            return new CreatedAtRouteResult("GetCategoryById", new { id = createdCategory.CategoryId }, createdCategory);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao criar categoria");
            return StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao criar categoria. Detalhes: {e.Message}");
        }
    }

    [HttpDelete("{id}")]
    public ActionResult Delete(int id)
    {
        Category? category = null;
        try
        {
            category = repository.Get(c => c.CategoryId == id);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao buscar categoria para exclusão. Id: {Id}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Erro ao buscar categoria para exclusão \nError: {e.Message}");
        }
    
        if (category is null)
        {
            logger.LogInformation("Categoria com id {Id} não encontrada para exclusão.", id);
            return NotFound($"Category with id= {id} not found.");
        }
    
        try
        {
            repository.Delete(category);
            return NoContent();
        }
        catch (DbUpdateException dbEx)
        {
            logger.LogError(dbEx, "Erro de integridade referencial ao deletar categoria. Id: {Id}", id);
            return StatusCode(
                StatusCodes.Status409Conflict,
                $"Não foi possível deletar a categoria devido a restrições de integridade referencial. Detalhes: {dbEx.Message}");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Erro ao deletar categoria. Id: {Id}", id);
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                $"Erro ao deletar categoria \nError: {e.Message}");
        }
    }
}