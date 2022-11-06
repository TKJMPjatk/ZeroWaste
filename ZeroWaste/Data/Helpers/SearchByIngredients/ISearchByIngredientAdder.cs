using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Helpers.SearchByIngredients;

public interface ISearchByIngredientAdder
{
    void AddIngredientToSearchByIngredientsVm(SearchByIngredientsVm searchByIngredients);
}