

using APICatalogo.Context;
using APICatalogo.Model;
using APICatalogo.Repository.Implementation;

namespace APICatalogo.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
            
        }
    }
}
