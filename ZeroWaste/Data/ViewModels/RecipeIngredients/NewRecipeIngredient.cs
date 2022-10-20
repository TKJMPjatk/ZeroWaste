using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZeroWaste.Data.ViewModels.RecipeIngredients
{
    public class NewRecipeIngredient : IValidatableObject
    {
        [Display(Name = "Nazwa istniejącego składnika")]
        public int ExistingIngredientId { get; set; }
        [Display(Name = "Jednostka miary")]
        public int ExistingIngredientUnitOfMeasureId { get; set; }
        [Display(Name = "Ilość")]
        public double ExistingIngredientQuantity { get; set; }
        [Display(Name = "Nazwa nowego składnika")]
        public string NewIngredientName { get; set; }
        [Display(Name = "Jednostka miary")]
        public int NewIngredientUnitOfMeasureId { get; set; }
        [Display(Name = "Ilość")]
        public double NewIngredientQuantity { get; set; }
        [Display(Name = "Typ produktu")]
        public int NewIngredientTypeId { get; set; }
        public int RecipeId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExistingIngredientId <= 0 && string.IsNullOrEmpty(NewIngredientName))
            {
                yield return new ValidationResult("Nie wskazano nowego ani istniejącego składnika.");
            }
            if (ExistingIngredientId > 0 && ExistingIngredientQuantity <= 0)
            {
                yield return new ValidationResult("Nie uzupełniono ilości dla istniejącego składnika.");
            }
            if (!string.IsNullOrEmpty(NewIngredientName) && NewIngredientUnitOfMeasureId <= 0)
            {
                yield return new ValidationResult("Nie uzupełniono jednostki miary dla nowego składnika.");
            }
            if (!string.IsNullOrEmpty(NewIngredientName) && NewIngredientQuantity <= 0)
            {
                yield return new ValidationResult("Nie uzupełniono jednostki miary dla nowego składnika.");
            }
            if (!string.IsNullOrEmpty(NewIngredientName) && NewIngredientTypeId <= 0)
            {
                yield return new ValidationResult("Nie uzupełniono typu dla nowego składnika.");
            }
        }
    }
}
