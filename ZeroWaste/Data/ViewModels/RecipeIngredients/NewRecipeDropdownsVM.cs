using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.RecipeIngredients
{
    public class RecipeIngredientsDropdownsVM
    {
        public List<Ingredient> Ingredients { get; set; }
        public List<UnitOfMeasure> UnitOfMeasures { get; set; }
    }
}
