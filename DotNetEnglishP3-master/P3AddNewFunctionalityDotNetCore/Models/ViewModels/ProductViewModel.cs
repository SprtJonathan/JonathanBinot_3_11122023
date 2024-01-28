using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace P3AddNewFunctionalityDotNetCore.Models.ViewModels
{
    public class ProductViewModel
    {
        [BindNever]
        public int Id { get; set; }

        [Required(ErrorMessage = "MissingName")]
        public string Name { get; set; }

        [Required(ErrorMessage = "MissingPrice")]
        public string Description { get; set; }

        public string Details { get; set; }

        [Required(ErrorMessage = "MissingQuantity")]
        [RegularExpression(@"^\d+$", ErrorMessage = "QuantityNotAnInteger")]
        [Range(1, int.MaxValue, ErrorMessage = "QuantityNotGreaterThanZero")]
        public string Stock { get; set; }

        [Required(ErrorMessage = "PriceNotANumber")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "PriceNotANumber")]
        [Range(0.01, double.MaxValue, ErrorMessage = "PriceNotGreaterThanZero")]
        public string Price { get; set; }
    }
}
