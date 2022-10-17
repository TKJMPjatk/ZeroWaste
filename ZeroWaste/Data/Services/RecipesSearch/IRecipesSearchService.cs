using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public interface IRecipesSearchService
{
    Task<SearchRecipeResultsVm> GetByCategoryAsync(int categoryId);
    Task<SearchRecipeResultsVm> GetByIngredients(List<IngredientsForSearchVM> ingredientsForSearchVm);
}