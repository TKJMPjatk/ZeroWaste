using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Data.Helpers;

public interface IUrlQueryHelper
{
    List<IngredientsForSearchVM> GetIngredientsFromUrl(string url);
}