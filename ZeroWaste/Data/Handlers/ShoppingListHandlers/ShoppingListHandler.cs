using AutoMapper;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.ShoppingListHandlers;

public class ShoppingListHandler : IShoppingListHandler
{
    private IShoppingListsService _shoppingListsService;
    private IMapper _mapper;
    public ShoppingListHandler(IShoppingListsService shoppingListsService, IMapper mapper)
    {
        _shoppingListsService = shoppingListsService;
        _mapper = mapper;
    }
    public async Task<List<ShoppingList>> GetShoppingListsByUserId(string userId)
    {
        return await _shoppingListsService
            .GetByUserIdAsync(userId);
    }
    public async Task<ShoppingList> GetShoppingListById(int id)
    {
        return await _shoppingListsService
            .GetByIdAsync(id);
    }
    public async Task<ShoppingList> Create(NewShoppingListVM shoppingListVm, string userId)
    {
        ShoppingList shoppingList = MapNewShoppingListVm(shoppingListVm);
        shoppingList.UserId = userId;
        shoppingList.FillTodaysDate();
        var addedShoppingList = await _shoppingListsService.CreateAsync(shoppingList);
        return addedShoppingList;
    }
    private ShoppingList MapNewShoppingListVm(NewShoppingListVM shoppingListVm)
    {
        ShoppingList shoppingList = _mapper.Map<ShoppingList>(shoppingListVm);
        return shoppingList;
    }
    public async Task DeleteAsync(int id)
    {
        await _shoppingListsService.DeleteAsync(id);
    }
    public async Task<bool> IsZeroQuantityIngredientsExists(int shoppingListId)
    {
        return await _shoppingListsService.IsZeroQuantityIngredientsExists(shoppingListId);
    }
    public async Task<bool> IsShoppingListExists(int shoppingListId)
    {
        return await _shoppingListsService.IsShoppingListExists(shoppingListId);
    }
}