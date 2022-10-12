using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipeIngredients
{
    public interface IRecipeIngredientService
    {
        Task<RecipeIngredientsDropdownsVM> GetDropdownsValuesAsync();
        Task<IEnumerable<RecipeIngredient>> GetCurrentIngredientsAsync(int? recipeId);
    }
}
