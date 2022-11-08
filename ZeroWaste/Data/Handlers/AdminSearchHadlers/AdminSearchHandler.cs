using ZeroWaste.Data.Enums;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Data.Handlers.AdminSearchHadlers;

public class AdminSearchHandler : IAdminSearchHandler
{
    private readonly IRecipesService _recipesService;
    public AdminSearchHandler(IRecipesService recipesService)
    {
        _recipesService = recipesService;
    }
    public async Task<SearchRecipeResultsVm> ConfirmRecipe(int recipeId)
    {
        await _recipesService.ConfirmRecipe(recipeId);
        return new SearchRecipeResultsVm()
        {
            SearchType = SearchType.Admin
        };
    }
}