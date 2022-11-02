using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.Services.RecipeService;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.AutomatedShoppingList;

public class AutomatedShoppingListHandler : IAutomatedShoppingListHandler
{
    private IRecipeService _recipeService;
    private IShoppingListsService _shoppingListsService;
    private AppDbContext _context;
    public AutomatedShoppingListHandler(IRecipeService recipeService, IShoppingListsService shoppingListsService, AppDbContext context)
    {
        _recipeService = recipeService;
        _shoppingListsService = shoppingListsService;
        _context = context;
    }
    public async Task<ShoppingList> AddNewShoppingList(int recipeId, string userId)
    {
        var recipe = await GetRecipe(recipeId);
        ShoppingList shoppingList = new ShoppingList()
        {
            Title = recipe.Title,
            Note = $"Wygenerowane automatycznie z przepisu {recipe.Title}",
            UserId = userId
        };
        shoppingList = await AddShoppingList(shoppingList);
        var ingredientsList = await GetRecipeIngredientsForRecipe(recipeId);
        await AddIngredientsToShoppingList(ingredientsList, shoppingList.Id);
        return shoppingList;
    }

    private async Task<Recipe> GetRecipe(int id)
    {
        var item = await _recipeService
            .GetByIdAsync(id);
        return item;
    }
    private async Task<ShoppingList> AddShoppingList(ShoppingList shoppingList)
    {
        var item = await _shoppingListsService.CreateAsync(shoppingList);
        return item;
    }
    private async Task<List<RecipeIngredient>> GetRecipeIngredientsForRecipe(int recipeId)
    {
        var ingredientsList = await _context
            .RecipeIngredients
            .Where(x => x.RecipeId == recipeId)
            .ToListAsync();
        return ingredientsList;
    }

    private async Task AddIngredientsToShoppingList(List<RecipeIngredient> recipeIngredients, int shoppingListId)
    {
        List<ShoppingListIngredient> shoppingListIngredients = new List<ShoppingListIngredient>();
        foreach (var item in recipeIngredients)
        {
            shoppingListIngredients.Add(new ShoppingListIngredient()
            {
                IngredientId = item.IngredientId,
                Quantity = item.Quantity,
                Selected = false,
                ShoppingListId = shoppingListId
            });
        }
        await _context.ShoppingListIngredients.AddRangeAsync(shoppingListIngredients);
        await _context.SaveChangesAsync();
    }
}