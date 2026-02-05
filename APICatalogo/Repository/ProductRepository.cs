using APICatalogo.Context;
using APICatalogo.Model;

namespace APICatalogo.Repository
{
    public class ProductRepository(AppDbContext context, ILogger logger) : IProductRepository
    {
        public Product CreateProduct(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }
            context.Products.Add(product);
            context.SaveChanges();

            return product;
        }

        public Product DeleteProduct(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == id);
            if (context is null)
                logger.LogWarning("Product with id {Id} not found for deletion", id);
            context.Products.Remove(product);
            context.SaveChanges();

            return product;
        }

        public Product GetProductById(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.ProductId == id);
            if (product is null)
            {
                logger.LogWarning("Product with id {Id} not found", id);
            }
            return product;
        }

        public IEnumerable<Product> GetProducts()
        {
            var products = context.Products.ToList();
            if (products is null || !products.Any())
            {
                logger.LogWarning("No products found in the database");
            }

            return products;
        }

        public Product UpdateProduct(Product product)
        {
            if (product is null)
            {
                throw new ArgumentNullException(nameof(product), "Product cannot be null");
            }
            context.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();

            return product;
        }
    }
}
