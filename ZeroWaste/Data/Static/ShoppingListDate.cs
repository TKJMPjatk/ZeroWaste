using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Static;

public static class ShoppingListDate
{
    public static void FillTodaysDate(this ShoppingList shoppingList)
    {
        shoppingList.CreatedAt = DateTime.Now;
    }
}