using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.ShoppingLists;

public class ShoppingListIngredientService : IShoppingListIngredientService
{
    private readonly AppDbContext _context;
    public ShoppingListIngredientService(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddIngredientToShoppingList(int shoppingListId, int ingredientId)
    {
        ShoppingListIngredient entity = new ShoppingListIngredient
        {
            IngredientId = ingredientId,
            ShoppingListId = shoppingListId,
            Quantity = 0
        };
        await _context.ShoppingListIngredients.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public Task AddIngredientToShoppingList(int shoppingListId, int ingredientId, int quantity)
    {
        throw new NotImplementedException();
    }
}