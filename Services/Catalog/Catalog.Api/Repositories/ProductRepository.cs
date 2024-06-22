using Catalog.Api.Data;
using Catalog.Api.Entities;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.Api.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region Constructor's

        private readonly ICatalogContext _catalogContext;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _catalogContext = catalogContext;
        }

        #endregion

        #region Methods

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var result = await _catalogContext.Products
                .Find(p => true)
                .ToListAsync();

            return result;
        }

        public async Task<Product> GetByIdAsync(string id)
        {
            var result = await _catalogContext.Products
                .Find(p => p.Id == id)
                .FirstOrDefaultAsync();

            return result;
        }

        public async Task<IEnumerable<Product>> GetByNameAsync(string name)
        {
            var filter = Builders<Product>.Filter
                .Eq(p => p.Name, name);

            var result = await _catalogContext.Products
                .Find(filter: filter)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(string categoryName)
        {
            var filter = Builders<Product>.Filter
                .Eq(p => p.Category, categoryName);

            var result = await _catalogContext.Products
                .Find(filter: filter)
                .ToListAsync();

            return result;
        }

        public async Task CreateProductAsync(Product product)
        {
            await _catalogContext.Products
                .InsertOneAsync(document: product);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var updateResult = await _catalogContext.Products
                .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

            return updateResult.IsAcknowledged &&
                updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProductAsync(string id)
        {
            var filter = Builders<Product>.Filter
                .Eq(p => p.Id, id);

            var deleteResult = await _catalogContext.Products
                .DeleteOneAsync(filter: filter);

            return deleteResult.IsAcknowledged &&
                deleteResult.DeletedCount > 0;
        }


        #endregion
    }
}
