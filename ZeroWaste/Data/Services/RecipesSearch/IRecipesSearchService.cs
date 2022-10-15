using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public interface IRecipesSearchService
{
    Task<List<Recipe>> GetByCategoryAsync(int categoryId);
    Task<List<Recipe>> GetByIngredients(List<IngredientsForSearchVM> ingredientsForSearchVm);
}