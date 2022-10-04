using ZeroWaste.Data.ViewModels.ShoppingList;

namespace ZeroWaste.Data.Helpers;

public class ShoppingListIngredientsHelper : IShoppingListIngredientsHelper
{
    private readonly AppDbContext _context;
    public ShoppingListIngredientsHelper(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ShoppingListIngredientsVM> GetShoppingListIngredients(int id)
    {
        ShoppingListIngredientsVM listIngredientsVm = new ShoppingListIngredientsVM()
        {
            ShoppingListId = id
        };
        return listIngredientsVm;
    }
}