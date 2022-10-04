using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

public class ShoppingListsController : Controller
{
    private readonly IShoppingListsService _shoppingListsService;
    public ShoppingListsController(IShoppingListsService shoppingListsService)
    {
        _shoppingListsService = shoppingListsService;
    }
    public async Task<IActionResult> Index()
    {
        List<ShoppingList> shoppingLists = await _shoppingListsService.GetAllAsync();
        return View(shoppingLists);
    }

    public async Task<IActionResult> Edit(int id)
    {
        return View();
    }
}