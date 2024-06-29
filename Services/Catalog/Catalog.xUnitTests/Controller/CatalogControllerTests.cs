using FakeItEasy;
using Catalog.Api.Repositories;
using Catalog.Api.Entities;
using Catalog.Api.Controllers;
using Microsoft.Extensions.Logging;
using FluentAssertions;

namespace Catalog.xUnitTests.Controller
{
    public class CatalogControllerTests
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogControllerTests()
        {
            _productRepository = A.Fake<IProductRepository>();
            _logger = A.Fake<ILogger<CatalogController>>();
        }

        [Fact]
        public void GetProducts_ReturnOK()
        {
            //Arange
            var controller = new CatalogController(_productRepository, _logger);

            //Act
            var result = controller.GetProducts();

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void GetById_ReturnOK()
        {
            //Arange
            var controller = new CatalogController(_productRepository, _logger);
            string id = "602d2149e773f2a3990b47f5";

            //Act
            var result = controller.GetProductById(id);

            //Assert
            result.Should().NotBeNull();
        }
    }
}
