using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Data.Helpers;

public class UrlQueryHelper : IUrlQueryHelper
{
    public List<IngredientsForSearchVM> GetIngredientsFromUrl(string url)
    {
        List<string> separatedUrl = SepareteString(url);
        List<IngredientsForSearchVM> ingredientsForSearchList = new List<IngredientsForSearchVM>();
        var newIngredientForSearch = new IngredientsForSearchVM();
        for (int i = 0; i < separatedUrl.Count(); i++)
        {
            if (i % 2 == 1)
            {
                newIngredientForSearch = new IngredientsForSearchVM();
                newIngredientForSearch.Name = separatedUrl[i];
            }
            else
            {
                newIngredientForSearch.Quantity = separatedUrl[i];
                ingredientsForSearchList.Add(newIngredientForSearch);
            }
        }
        return ingredientsForSearchList;
    }
    private List<string> SepareteString(string url)
    {
        var list = url.Split(',').ToList();
        return list;
    }
}