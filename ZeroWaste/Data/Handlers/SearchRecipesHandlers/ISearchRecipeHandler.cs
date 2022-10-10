using ZeroWaste.Data.ViewModels.CategorySearch;

namespace ZeroWaste.Data.Handlers.SearchRecipesHandlers;

public interface ISearchRecipeHandler
{
    Task<List<CategorySearchVm>> GetCategoriesSearchVm();
}