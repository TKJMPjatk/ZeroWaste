using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.ShoppingList;

public class ShoppingListIngredientsVm
{
    public int ShoppingListId { get; set; }
    public List<IngredientsToAddVm> Ingredients { get; set; } = new List<IngredientsToAddVm>();
    public List<IngredientsToAddVm> IngredientsToAddVm { get; set; } = new List<IngredientsToAddVm>();
    public List<IngredientType> IngredientTypes { get; set; }
    public int SelectedCategoryId { get; set; }
}