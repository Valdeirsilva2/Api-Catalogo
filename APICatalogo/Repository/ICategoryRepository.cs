using APICatalogo.Model;

namespace APICatalogo.Repository
{
    public interface ICategoryRepository
    {
        // IEnumerable retorna uma estrutura mais generica podendo retornar um List uma Collection ou um Dicionario
        IEnumerable<Category> GetCategories();
        Category GetCategory(int id);
        Category CreateCategory(Category category);
        Category UpdateCategory(Category category);
        Category DeleteCategory(int id);
    }
} 
