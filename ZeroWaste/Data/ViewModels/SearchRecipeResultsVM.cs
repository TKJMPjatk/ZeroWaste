using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Structs;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels;

public class SearchRecipeResultsVm
{
    public List<RecipeResult> RecipesList { get; set; } = new List<RecipeResult>();
    public List<RecipeResult> RecipesListBase { get; set; } = new List<RecipeResult>();
    public List<Category> CategoryList { get; set; }
    public List<IngredientForSearch> IngredientsLists { get; set; } = new List<IngredientForSearch>();
    public int CategoryId { get; set;  }
    public int SortTypeId { get; set; }
    public string PageTitle { get; set; }
    public int StatusId { get; set; }
    public SearchType SearchType { get; set; }
    public string UserId { get; set; }
    public string SearchSentence { get; set; }
}