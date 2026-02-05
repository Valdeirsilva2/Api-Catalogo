using APICatalogo.Context;
using APICatalogo.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repository
{
    public class CategoryRepository(AppDbContext context, ILogger logger) : ICategoryRepository
    {

        public IEnumerable<Category> GetCategories()
        {
            var categories = context.Categories.ToList();
            if (categories.Count == 0)
            {
                logger.LogInformation("No categories found in the database.");
            }
            return categories;
        }

        public Category? GetCategory(int id)
        {
            var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category == null)
            {
                logger.LogInformation("Category not found. Id: {CategoryId}", id);
            }

            return category;
        }
        public Category CreateCategory(Category category)
        {
            if (category is null)
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");

            context.Categories.Add(category); // Adiciona a nova categoria ao DbSet em memória
            context.SaveChanges(); // Persiste as mudanças no banco de dados

            return category;

        }
        public Category UpdateCategory(Category category)
        {
            if (category is null)
                throw new ArgumentException(nameof(category), "Category ID mismatch.");

            context.Entry(category).State = EntityState.Modified;// Marca a entidade como modificada em memória
            context.SaveChanges();

            return category;
        }
        public Category DeleteCategory(int id)
        {
           var category = context.Categories.FirstOrDefault(c => c.CategoryId == id);
            if (category is null)
                throw new ArgumentNullException(nameof(category), "Category not found.");
            context.Categories.Remove(category); // Remove a entidade do DbSet em memória
            context.SaveChanges(); // Persiste as mudanças no banco de dados
            return category;
        }

    }
}
