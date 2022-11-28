using ZeroWaste.Data.ViewModels.RecipeIngredients;

namespace ZeroWaste.Data.Handlers.RecipeIngredientsHandlers
{
    public interface IRecipeIngredientsHandler
    {
        Task AddIngredient(NewRecipeIngredient newRecipeIngredient);
    }
}
