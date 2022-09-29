using System.Text.Json.Serialization;

namespace ZeroWaste.Data.ViewModels.Recipes;

public class SearchByIngredientsVm
{
    //[JsonPropertyName("Ingredients")]
    public List<IngredientsForSearchVM> IngredientsList{ get; set; }
    //public List<string> IngredientsList { get; set; }
}