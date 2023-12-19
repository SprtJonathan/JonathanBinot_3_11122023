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
        }

    }
}