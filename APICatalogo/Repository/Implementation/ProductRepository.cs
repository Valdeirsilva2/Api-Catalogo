using APICatalogo.Context;
using APICatalogo.Model;
using APICatalogo.Repository.Implementation;

namespace APICatalogo.Repository
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(AppDbContext context) : base(context)
        {
        }

        public IEnumerable<Product> GetCategoriesWithProducts(int id)
        {
            return GetAll().Where(c => c.CategoryId == id);
        }
    }
}
