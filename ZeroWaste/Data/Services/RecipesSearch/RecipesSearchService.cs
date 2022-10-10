using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.RecipesSearch;

public class RecipesSearchService : IRecipesSearchService
{
    public Task<List<Recipe>> GetByCategoryAsync(int categoryId)
    {
        throw new NotImplementedException();
    }
}