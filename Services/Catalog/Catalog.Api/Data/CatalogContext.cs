using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; set; }

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetConnectionString("CatalogConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("ConnectionStrings:DatabaseName"));

            Products = database.GetCollection<Product>(configuration.GetValue<string>("ConnectionStrings:CollectionName"));

            CatalogContextSeed.SeedData(Products);
        }

    }
}
