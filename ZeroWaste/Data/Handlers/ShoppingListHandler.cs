using AutoMapper;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers;

public class ShoppingListHandler : IShoppingListHandler
{
    private IShoppingListsService _shoppingListsService;
    private IMapper _mapper;
    public ShoppingListHandler(IShoppingListsService shoppingListsService, IMapper mapper)
    {
        _shoppingListsService = shoppingListsService;
        _mapper = mapper;
    }
    public async Task Create(NewShoppingListVM shoppingListVm)
    {
        ShoppingList shoppingList = MapNewShoppingListVm(shoppingListVm);
        await _shoppingListsService.CreateAsync(shoppingList);
    }
    private ShoppingList MapNewShoppingListVm(NewShoppingListVM shoppingListVm)
    {
        ShoppingList shoppingList = _mapper.Map<ShoppingList>(shoppingListVm);
        return shoppingList;
    }
}