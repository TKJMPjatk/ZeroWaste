namespace ZeroWaste.Data.Handlers.AutomatedShoppingList;

public interface IAutomatedShoppingListHandler
{
    Task AddNewShoppingList(int recipeId);
}