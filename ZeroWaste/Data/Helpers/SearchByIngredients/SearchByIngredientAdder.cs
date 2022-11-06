using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels.RecipeSearch;

namespace ZeroWaste.Data.Helpers.SearchByIngredients;

public class SearchByIngredientAdder : ISearchByIngredientAdder
{
    public void AddIngredientToSearchByIngredientsVm(SearchByIngredientsVm searchByIngredients)
    {
        int ingredientsListQuantity = searchByIngredients.SingleIngredientToSearchVm.Count;        
        searchByIngredients.SingleIngredientToSearchVm.Add(new IngredientForSearch() 
        {
            Name = searchByIngredients.Name, 
            Quantity = searchByIngredients.Quantity, 
            Index = ingredientsListQuantity+1,
        }); 
        searchByIngredients.Name = string.Empty; 
        searchByIngredients.Quantity = 0; 
    }
}