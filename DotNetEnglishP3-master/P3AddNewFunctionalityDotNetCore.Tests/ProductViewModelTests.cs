using Xunit;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductViewModelTests
    {
        // Méthode utilitaire pour tester la validation
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var validationContext = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, validationContext, validationResults, true);
            return validationResults;
        }

        [Fact]
        public void ProductViewModel_Valid()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Nom Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "100"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Empty(results);
        }

        [Fact]
        public void ProductViewModel_RequiredName()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = null, // Nom manquant
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "50"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "MissingName");
        }

        [Fact]
        public void ProductViewModel_InvalidPrice()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Nom Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "abc", // Prix invalide (doit être un nombre)
                Stock = "50"
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "PriceNotANumber");
            Assert.Contains(results, v => v.ErrorMessage == "PriceNotGreaterThanZero");
        }

        [Fact]
        public void ProductViewModel_InvalidStockOverMaxInt()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Nom Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "9999999999999" // Valeur dépassant la limite autorisée pour les integer en C#
            };

            // Act
            System.OverflowException ex = Assert.Throws<System.OverflowException>(() => ValidateModel(product));

            // Assert
            Assert.Equal("Value was either too large or too small for an Int32.", ex.Message);
        }


        [Fact]
        public void ProductViewModel_InvalidStock()
        {
            // Arrange
            var product = new ProductViewModel
            {
                Name = "Nom Valide",
                Description = "Description Valide",
                Details = "Details Valide",
                Price = "100",
                Stock = "abc" // Stock invalide (doit être un entier)
            };

            // Act
            var results = ValidateModel(product);

            // Assert
            Assert.Contains(results, v => v.ErrorMessage == "QuantityNotAnInteger");
            Assert.Contains(results, v => v.ErrorMessage == "QuantityNotGreaterThanZero");
        }
    }
}
