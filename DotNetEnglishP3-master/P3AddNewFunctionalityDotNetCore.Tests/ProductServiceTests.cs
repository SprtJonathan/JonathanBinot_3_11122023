using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {

        private readonly IStringLocalizer<ProductService> _localizer;
        public ProductServiceTests()
        {
            // Initialisez _localizer ici. Cela pourrait être fait avec une bibliothèque de localisation réelle ou un mock.
            // Dans cet exemple, un mock est utilisé à l'aide de Moq.
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            // Ajoutez d'autres initialisations pour les clés de localisation utilisées dans vos tests.

            _localizer = mockLocalizer.Object;
        }

        static List<string> validityResult(ProductViewModel product)
        {
            var mockCart = Mock.Of<ICart>();
            var mockProductRepository = Mock.Of<IProductRepository>();
            var mockOrderRepository = Mock.Of<IOrderRepository>();
            var mockLocalizer = Mock.Of<IStringLocalizer<ProductService>>();

            var productService = new ProductService(mockCart, mockProductRepository, mockOrderRepository, mockLocalizer);

            return productService.CheckProductModelErrors(product);
        }

        // Test de la méthode CheckProductModelErrors de ProductService avec un produit valide
        [Fact]
        public void CheckProductModelErrors_ValidData()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Name Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "50"
            };

            // Act
            var result = validityResult(product);

            // Assert
            Assert.Empty(result);
        }

        // Test de la méthode CheckProductModelErrors de ProductService avec un produit sans nom
        [Fact]
        public void CheckProductModelErrors_NoName()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "50"
            };

            // Act
            var result = validityResult(product);

            // Assert
            Assert.Single(result);
            Assert.Contains(_localizer["MissingName"], result);
        }

        [Fact]
        public void CheckProductModelErrors_InvalidPrice()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Name Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "Invalid Price",
                Stock = "50"
            };

            // Act
            var result = validityResult(product);

            // Assert
            Assert.Single(result);
            Assert.Contains(_localizer["PriceNotANumber"], result);
        }

        [Fact]
        public void CheckProductModelErrors_InvalidStock()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Name Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "Invalid Stock"
            };

            // Act
            var result = validityResult(product);

            // Assert
            Assert.Single(result);
            Assert.Contains(_localizer["StockNotAnInteger"], result);
        }

        [Fact]
        public void CheckProductModelErrors_InvalidData()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "",
                Description = "",
                Details = "",
                Price = "",
                Stock = ""
            };

            // Act
            var result = validityResult(product);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains(_localizer["MissingName"], result);
            Assert.Contains(_localizer["MissingDescription"], result);
            Assert.Contains(_localizer["MissingDetails"], result);
            Assert.Contains(_localizer["StockNotAnInteger"], result);
            Assert.Contains(_localizer["PriceNotANumber"], result);
        }


    }
}