using ZeroWaste.Data.Structs;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels;

public class SearchRecipeResultsVm
{
    public List<RecipeResult> RecipesList { get; set; }
    public List<Category> CategoryList { get; set; }
    public List<IngredientForSearch> IngredientsLists { get; set; }
    public int CategoryId { get; set;  }
    public int SortTypeId { get; set; }
    public string PageTitle { get; set; }
}