using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.RecipeIngredients
{
    public class RecipeIngredientsDropdownsVM
    {
        public IEnumerable<ExistingIngredient> Ingredients { get; set; }
        public List<UnitOfMeasure> UnitOfMeasures { get; set; }
    }
}
