using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Moq;
using NuGet.ContentModel;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {

        private static IStringLocalizer<ProductService> _localizer;
        public ProductServiceTests()
        {
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();
            mockLocalizer.Setup(_ => _["MissingName"]).Returns(new LocalizedString("MissingName", "Please enter a name"));
            mockLocalizer.Setup(_ => _["MissingPrice"]).Returns(new LocalizedString("MissingPrice", "Please enter a price"));
            mockLocalizer.Setup(_ => _["MissingQuantity"]).Returns(new LocalizedString("MissingQuantity", "Please enter a stock value"));
            mockLocalizer.Setup(_ => _["PriceNotANumber"]).Returns(new LocalizedString("PriceNotANumber", "The value entered for the price must be a number"));
            mockLocalizer.Setup(_ => _["PriceNotGreaterThanZero"]).Returns(new LocalizedString("PriceNotGreaterThanZero", "The price must be greater than zero"));
            mockLocalizer.Setup(_ => _["StockNotAnInteger"]).Returns(new LocalizedString("StockNotAnInteger", "The value entered for the stock must be a integer"));
            mockLocalizer.Setup(_ => _["StockNotGreaterThanZero"]).Returns(new LocalizedString("StockNotGreaterThanZero", "The stock must greater than zero"));
            _localizer = mockLocalizer.Object;
        }

        private List<Product> GetMockedProducts()
        {
            return new List<Product>
            {
                new Product { Id = 1, Name = "Produit 1", Price = 10.99, Quantity = 100, Description = "Description 1", Details = "Détails 1" },
                new Product { Id = 2, Name = "Produit 2", Price = 20.99, Quantity = 50, Description = "Description 2", Details = "Détails 2" },
                new Product { Id = 3, Name = "Produit 3", Price = 30.99, Quantity = 25, Description = "Description 3", Details = "Détails 3" },
            };
        }

        static List<string> ValidityResult(ProductViewModel product)
        {
            var mockCart = Mock.Of<ICart>();
            var mockProductRepository = Mock.Of<IProductRepository>();
            var mockOrderRepository = Mock.Of<IOrderRepository>();

            var productService = new ProductService(mockCart, mockProductRepository, mockOrderRepository, _localizer);

            return productService.CheckProductModelErrors(product);
        }
        /// <summary>
        /// CheckProductModelErrors
        /// </summary>
        /// 
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
            var result = ValidityResult(product);

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
            var result = ValidityResult(product);

            // Assert
            Assert.Single(result);
            Assert.Contains(_localizer["MissingName"], result);
        }

        // Test de la méthode CheckProductModelErrors de ProductService avec un prix invalide
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
            var result = ValidityResult(product);

            // Assert
            Assert.Single(result);
            Assert.Contains(_localizer["PriceNotANumber"], result);
        }

        // Test de la méthode CheckProductModelErrors de ProductService avec un stock (quantité) invalide
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
                Stock = "9999999999999" // Valeur dépassant la limite autorisée pour les integer en C#
            };

            // Act
            var result = ValidityResult(product);

            // Assert
            Assert.Single(result);
            Assert.Contains(_localizer["QuantityNotAnInteger"], result);
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
            var result = ValidityResult(product);

            // Assert
            Assert.Equal(5, result.Count);
            Assert.Contains(_localizer["MissingName"], result);
            Assert.Contains(_localizer["MissingPrice"], result);
            Assert.Contains(_localizer["PriceNotANumber"], result);
            Assert.Contains(_localizer["MissingQuantity"], result);
            Assert.Contains(_localizer["QuantityNotAnInteger"], result);
        }

        /// <summary>
        /// SetupProductService
        /// </summary>
        private ProductService SetupProductService()
        {
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(GetMockedProducts());

            var productService = new ProductService(Mock.Of<ICart>(), mockProductRepository.Object, Mock.Of<IOrderRepository>(), _localizer);
            return productService;
        }


        /// <summary>
        /// GetAllProductsViewModel
        /// </summary>
        /// 
        [Fact]
        public void GetAllProductsViewModel_ReturnsViewModelList()
        {
            // Arrange
            ProductService productService = SetupProductService();

            // Act
            var result = productService.GetAllProductsViewModel();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<ProductViewModel>>(result);
            Assert.Equal(3, result.Count);
            Assert.Equal(GetMockedProducts()[0].Name, result[0].Name);
            Assert.Equal(GetMockedProducts()[1].Name, result[1].Name);
            Assert.Equal(GetMockedProducts()[2].Name, result[2].Name);
        }       

        /// <summary>
        /// GetProduct(int id)
        /// </summary>
        /// 
        // Id produit valide
        [Fact]
        public async Task GetProduct_ValidId_ReturnsExpectedProduct()
        {
            // Arrange
            int productId = 1; // ID d'un produit existant
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(repo => repo.GetProduct(productId))
                                 .ReturnsAsync(new Product { Id = productId, Name = "Produit Test" });

            var productService = new ProductService(Mock.Of<ICart>(), mockProductRepository.Object, Mock.Of<IOrderRepository>(), _localizer);

            // Act
            var result = await productService.GetProduct(productId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<Product>(result);
            Assert.Equal(productId, result.Id);
            Assert.Equal("Produit Test", result.Name);
        }

        // Id produit invalide
        [Fact]
        public async Task GetProduct_InvalidId_ReturnsNull()
        {
            // Arrange
            int productId = 999; // ID d'un produit inexistant
            var mockProductRepository = new Mock<IProductRepository>();
            mockProductRepository.Setup(repo => repo.GetProduct(productId))
                                 .ReturnsAsync((Product)null);

            var productService = new ProductService(Mock.Of<ICart>(), mockProductRepository.Object, Mock.Of<IOrderRepository>(), _localizer);

            // Act
            var result = await productService.GetProduct(productId);

            // Assert
            Assert.Null(result);
        }
    }

}
