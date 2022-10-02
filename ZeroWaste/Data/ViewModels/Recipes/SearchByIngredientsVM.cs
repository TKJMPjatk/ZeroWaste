using System.Text.Json.Serialization;

namespace ZeroWaste.Data.ViewModels.Recipes;

public class SearchByIngredientsVm
{
    public List<IngredientsForSearchVM> IngredientsList{ get; set; }
}