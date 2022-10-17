using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels;

public class SearchRecipeResultsVm
{
    public List<Recipe> RecipeList { get; set; }
    public List<Category> CategoryList { get; set; }
    public List<IngredientsForSearchVM> IngredientForSearchList { get; set; }
    public int CategoryId { get; set;  }
    public int SortType { get; set; }
    public string PageTitle { get; set; }
}