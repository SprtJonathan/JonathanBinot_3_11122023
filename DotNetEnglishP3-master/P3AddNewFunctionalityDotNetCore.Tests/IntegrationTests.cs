using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Moq;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.IntegrationTests
{
    public class ProductServiceIntegrationTests : IDisposable
    {
        private readonly P3Referential _context;
        private readonly ProductService _productService;
        private readonly ProductRepository _productRepository;

        public ProductServiceIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseInMemoryDatabase(databaseName: "P3Referential")
                .Options;
            _context = new P3Referential(options, null);

            _productRepository = new ProductRepository(_context);

            var mockCart = new Mock<ICart>();
            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            _productService = new ProductService(mockCart.Object, _productRepository, mockOrderRepository.Object, mockLocalizer.Object);
        }

        [Fact]
        public void SaveAndGetProduct_IntegrationTest()
        {
            // Arrange
            var productViewModel = new ProductViewModel
            {
                Name = "New Test Product",
                Description = "New Test Description",
                Details = "New Test Details",
                Price = "20,50",
                Stock = "15"
            };

            // Act
            _productService.SaveProduct(productViewModel);

            var savedProduct = _context.Product.FirstOrDefault(p => p.Name == productViewModel.Name);
            Assert.NotNull(savedProduct);

            // Act
            var fetchedProduct = _productService.GetProductById(savedProduct.Id);

            // Assert
            Assert.NotNull(fetchedProduct);
            Assert.Equal(productViewModel.Name, fetchedProduct.Name);
            Assert.Equal(productViewModel.Description, fetchedProduct.Description);
            Assert.Equal(productViewModel.Details, fetchedProduct.Details);
            Assert.Equal(double.Parse(productViewModel.Price), fetchedProduct.Price);
            Assert.Equal(int.Parse(productViewModel.Stock), fetchedProduct.Quantity);
        }

        [Fact]
        public void DeleteProduct_ProductIsDeleted()
        {
            // Arrange
            var productViewModel = new ProductViewModel
            {
                Name = "Product to Delete",
                Description = "Description",
                Details = "Details",
                Price = "20,50",
                Stock = "10"
            };

            // Act
            _productService.SaveProduct(productViewModel);

            var savedProduct = _context.Product.FirstOrDefault(p => p.Name == productViewModel.Name);
            Assert.NotNull(savedProduct);

            // Act
            _productService.DeleteProduct(savedProduct.Id);

            // Assert
            var deletedProduct = _context.Product.Find(savedProduct.Id);
            Assert.Null(deletedProduct);
        }
        
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
