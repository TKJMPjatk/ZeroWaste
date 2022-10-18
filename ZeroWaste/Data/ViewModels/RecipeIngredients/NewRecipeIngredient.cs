using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZeroWaste.Data.ViewModels.RecipeIngredients
{
    public class NewRecipeIngredient : IValidatableObject
    {
        [Display(Name = "Nazwa istniejącego składnika")]
        public int? ExistingIngredientId { get; set; }
        [Display(Name = "Jednostka miary")]
        public int? ExistingIngredientUnitOfMeasureId { get; set; }
        [Display(Name = "Ilość")]
        public double? ExistingIngredientQuantity { get; set; }
        [Display(Name = "Nazwa nowego składnika")]
        public string? NewIngredientName { get; set; }
        [Display(Name = "Jednostka miary")]
        public int? NewIngredientUnitOfMeasureId { get; set; }
        [Display(Name = "Ilość")]
        public double? NewIngredientQuantity { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExistingIngredientId is null || string.IsNullOrEmpty(NewIngredientName))
            {
                yield return new ValidationResult("Musisz wybrać istniejący składnik lub wpisać nazwę nowego.");
            }
            if (ExistingIngredientId.HasValue && !ExistingIngredientUnitOfMeasureId.HasValue)
            {
                yield return new ValidationResult("Nie uzupełniono jednostki miary dla istniejącego składnika.");
            }
            if (ExistingIngredientId.HasValue && !ExistingIngredientQuantity.HasValue)
            {
                yield return new ValidationResult("Nie uzupełniono ilości dla istniejącego składnika.");
            }
            if (!string.IsNullOrEmpty(NewIngredientName) && !NewIngredientUnitOfMeasureId.HasValue)
            {
                yield return new ValidationResult("Nie uzupełniono jednostki miary dla nowego składnika.");
            }
            if (!string.IsNullOrEmpty(NewIngredientName) && !NewIngredientQuantity.HasValue)
            {
                yield return new ValidationResult("Nie uzupełniono jednostki miary dla nowego składnika.");
            }
        }
    }
}
