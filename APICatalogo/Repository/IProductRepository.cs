using APICatalogo.Model;

namespace APICatalogo.Repository
{
    public interface IProductRepository : IRepository<Product>
    {
        IEnumerable<Product> GetCategoriesWithProducts(int id);
    }
}