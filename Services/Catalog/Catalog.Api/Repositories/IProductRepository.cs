using Catalog.Api.Entities;

namespace Catalog.Api.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetByIdAsync(string id);

        Task<IEnumerable<Product>> GetByNameAsync(string name);

        Task<IEnumerable<Product>> GetByCategoryAsync(string categoryName);

        Task CreateProductAsync(Product product);

        Task<bool> UpdateProductAsync(Product product);

        Task<bool> DeleteProductAsync(string id);
    }
}
