using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipeIngredients
{
    public interface IRecipeIngredientService
    {
        Task<RecipeIngredientsDropdownsVM> GetDropdownsValuesAsync();
        Task<IEnumerable<RecipeIngredient>> GetCurrentIngredientsAsync(int? recipeId);
        Task AddIngredientAsync(int recipeId, int ingredientId, double quantity);
        Task DeleteAsync(int id);
        Task<RecipeIngredient> GetByIdAsync(int id);
        Task<bool> RecipeIngredientsExistingAsync(int ingredientId);
        Task<bool> RecipeIngredientAlredyExistsAsync(int recipeId, int ingredientId);
        Task UpdateRecipeIngredientQuantity(int recipeId, int ingredientId, double quantity);
    }
}
