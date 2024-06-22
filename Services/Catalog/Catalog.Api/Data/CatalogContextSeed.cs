using Catalog.Api.Entities;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContextSeed
    {
        public static void SeedData(IMongoCollection<Product> products)
        {
            bool existsProduct = products
                .Find(p => true)
                .Any();

            if (existsProduct == false)
            {
                products.InsertManyAsync(GetSeedData());
            }
        }

        private static IEnumerable<Product> GetSeedData()
        {
            return new List<Product>()
            {
                new Product
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "IPhone X",
                    Summary = "Test Summary",
                    Description = "Test Description",
                    ImageFile = "IPhone.jpg",
                    Category = "Mobile",
                    Price = 499,
                },
                new Product
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung A70",
                    Summary = "Test Summary",
                    Description = "Test Description",
                    ImageFile = "Sumsung.jpg",
                    Category = "Mobile",
                    Price = 399,
                }
            };
        }
    }
}
