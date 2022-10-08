using AutoMapper;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers;

public class ShoppingListHandler : IShoppingListHandler
{
    private IShoppingListsService _shoppingListsService;
    private IIngredientSelectionService _ingredientSelectionService;
    private IMapper _mapper;
    public ShoppingListHandler(IShoppingListsService shoppingListsService, IIngredientSelectionService ingredientSelectionService,IMapper mapper)
    {
        _shoppingListsService = shoppingListsService;
        _ingredientSelectionService = ingredientSelectionService;
        _mapper = mapper;
    }
    public async Task<ShoppingList> Create(NewShoppingListVM shoppingListVm)
    {
        ShoppingList shoppingList = MapNewShoppingListVm(shoppingListVm);
        shoppingList.FillTodaysDate();
        var addedShoppingList = await _shoppingListsService.CreateAsync(shoppingList);
        return addedShoppingList;
    }

    public async Task HandleSelection(int ingredientId)
    {
        var ingredient = await _ingredientSelectionService
            .GetShoppingListIngredientByIdAsync(ingredientId);
        if (!(ingredient.Selected))
            await _ingredientSelectionService.SelectIngredient(ingredient);
        else
            await _ingredientSelectionService.UnSelectIngredient(ingredient);
    }

    private ShoppingList MapNewShoppingListVm(NewShoppingListVM shoppingListVm)
    {
        ShoppingList shoppingList = _mapper.Map<ShoppingList>(shoppingListVm);
        return shoppingList;
    }
}