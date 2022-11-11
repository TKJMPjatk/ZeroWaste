using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Helpers.SearchByIngredients;

public class SearchByIngredientAdder : ISearchByIngredientAdder
{
    public void AddIngredientToSearchByIngredientsVm(SearchByIngredientsVm searchByIngredients)
    {
        if (isIngredientExists(searchByIngredients.Name, searchByIngredients.SingleIngredientToSearchVm))
            AddExistedIngredient(searchByIngredients);
        else
            AddNotExistedIngredient(searchByIngredients);
        searchByIngredients.Name = string.Empty; 
        searchByIngredients.Quantity = 0; 
    }
    private bool isIngredientExists(string name, List<IngredientForSearch> ingredientsForSearch)
    {
        return ingredientsForSearch.Any(x => x.Name == name);
    }

    private void AddExistedIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        var item = searchByIngredientsVm
            .SingleIngredientToSearchVm
            .FirstOrDefault(x => x.Name == searchByIngredientsVm.Name);
        item.Quantity += searchByIngredientsVm.Quantity;
    }

    private void AddNotExistedIngredient(SearchByIngredientsVm searchByIngredientsVm)
    {
        int ingredientsListQuantity = searchByIngredientsVm.SingleIngredientToSearchVm.Count;
        searchByIngredientsVm.SingleIngredientToSearchVm.Add(new IngredientForSearch() 
        {
            Name = searchByIngredientsVm.Name, 
            Quantity = searchByIngredientsVm.Quantity,
            Unit = searchByIngredientsVm.Unit,
            Index = ingredientsListQuantity+1,
        }); 
    }
}