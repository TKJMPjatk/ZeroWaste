using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public interface IRecipesSearchService
{
    Task<List<Recipe>> GetByCategoryAsync(int categoryId);
}