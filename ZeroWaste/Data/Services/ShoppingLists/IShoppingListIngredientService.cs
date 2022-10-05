namespace ZeroWaste.Data.Services.ShoppingLists;

public interface IShoppingListIngredientService
{
    Task AddIngredientToShoppingList(int shoppingListId, int ingredientId);
    Task AddIngredientToShoppingList(int shoppingListId, int ingredientId, int quantity);
}