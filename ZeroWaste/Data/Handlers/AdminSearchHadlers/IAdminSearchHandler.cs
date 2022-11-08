using ZeroWaste.Data.ViewModels;

namespace ZeroWaste.Data.Handlers.AdminSearchHadlers;

public interface IAdminSearchHandler
{
    Task<SearchRecipeResultsVm> ConfirmRecipe(int recipeId);
}