using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipeService;

public interface IRecipeService
{
    Task<Recipe> GetByIdAsync(int id);
}