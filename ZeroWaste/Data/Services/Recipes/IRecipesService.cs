using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Data.ViewModels.NewRecepie;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Recipes
{
    public interface IRecipesService
    {
        Task<int> AddNewReturnsIdAsync(NewRecipeVM newRecipeVM, string userId);
        Task<Recipe> GetByIdAsync(int id);
        Task<RecipeDropdownVM> GetDropdownsValuesAsync();
        Task<DetailsRecipeVM> GetDetailsByIdAsync(int id);
        Task<EditRecipeVM> GetEditByIdAsync(int id);
        Task UpdateAsync(EditRecipeVM editRecipeVM, string userId);
        Task AddLiked(int recipeId, string userId);
        Task AddNotLiked(int recipeId, string userId);
        Task<bool> IsAuthorEqualsEditor(int recipeId, string editorId);
    }
}
