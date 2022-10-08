using ZeroWaste.Data.ViewModels.ShoppingListIngredients;

namespace ZeroWaste.Data.ViewModels.ShoppingList;

public class ShoppingListIngredientsVm
{
    public int ShoppingListId { get; set; }
    public List<IngredientsToAddVm> IngredientsToAddVm { get; set; } = new List<IngredientsToAddVm>();
}