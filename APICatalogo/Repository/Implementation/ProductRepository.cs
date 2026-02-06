using APICatalogo.Context;
using APICatalogo.Model;

namespace APICatalogo.Repository
{
    public class ProductRepository(AppDbContext context, ILogger<ProductRepository> logger) : IProductRepository
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

        public IEnumerable<Product> GetAllProducts()
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

            var existingProduct = context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);
            if (existingProduct is null)
            {
                logger.LogWarning("Product with id {Id} not found for update", product.ProductId);
                throw new InvalidOperationException("Product not found for update");
            }

            // Atualiza os campos
            existingProduct.Name = product.Name;
            existingProduct.Description = product.Description;
            existingProduct.Value = product.Value;
            existingProduct.Stock = product.Stock;
            existingProduct.ImageUrl = product.ImageUrl;
            existingProduct.CategoryId = product.CategoryId;

            context.SaveChanges();
            return existingProduct;
        }
    }
}
