using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Data.Services.HarmfulIngredients
{
    public interface IHarmfulIngredientsService
    {
        Task<IEnumerable<HarmfulIngredientVM>> GetHarmfulIngredientsForUser(string userId);
        Task<IEnumerable<HarmfulIngredientVM>> GetSafeIngredientsForUser(string userId);
        Task MarkIngredientAsHarmful(string userId, int ingredientId);
        Task UnmarkIngredientAsHarmful(string userId, int ingredientId);
    }
}
