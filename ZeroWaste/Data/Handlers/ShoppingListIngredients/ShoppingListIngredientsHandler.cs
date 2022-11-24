using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.ShoppingListIngredients;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.ShoppingListIngredients;

public class ShoppingListIngredientsHandler : IShoppingListIngredientsHandler
{
    private readonly IShoppingListIngredientsService _shoppingListIngredientService;
    private readonly IShoppingListsService _shoppingListsService;
    private readonly IIngredientsService _ingredientsService;
    private readonly IMapper _mapper;
    private List<int> _idOfIngredientsInShoppingList = new List<int>();
    public ShoppingListIngredientsHandler(IShoppingListIngredientsService shoppingListIngredientService, IIngredientsService ingredientsService, IShoppingListsService shoppingListsService, IMapper mapper)
    {
        _shoppingListIngredientService = shoppingListIngredientService;
        _ingredientsService = ingredientsService;
        _shoppingListsService = shoppingListsService;
        _mapper = mapper;
    }
    public async Task<ShoppingListIngredientsVm> GetShoppingListIngredientsVm(int shoppingListId, string searchString)
    {
        ShoppingListIngredientsVm shoppingListIngredientsVm = new ShoppingListIngredientsVm()
        {
            ShoppingListId = shoppingListId,
            IngredientsToAddVm = await _ingredientsService.GetIngredientsToAddVmAsync(shoppingListId, searchString)
        };
        return shoppingListIngredientsVm;
    }
    public async Task EditQuantityOfNewIngredients(EditQuantityVM editQuantityVm)
    {
        foreach (var item in editQuantityVm.IngredientsToEditQuantity)
        {
            await _shoppingListIngredientService
                .EditQuantityAsync(item.Id, item.Quantity);
        }
    }

    public async Task<int> ChangeShoppingListIngredientSelection(int shoppingListIngredientId)
    {
        int shoppingListId = await _shoppingListIngredientService.ChangeSelection(shoppingListIngredientId);
        return shoppingListId;
    }

    public async Task<int> HandleDeleteIngredientFromShoppingList(int shoppingListIngredientId)
    {
        int shoppingListId = (await _shoppingListsService.GetByShoppingListIngredientIdAsync(shoppingListIngredientId)).Id;
        await _shoppingListIngredientService.DeleteByIdAsync(shoppingListIngredientId);
        return shoppingListId;
    }

    public async Task AddIngredientToShoppingList(int ingredientId, int shoppingListId)
    {
        await _shoppingListIngredientService.AddAsync(shoppingListId, ingredientId);
    }

    public async Task<EditQuantityVM> GetEditQuantity(int shoppingListId)
    {
        return new EditQuantityVM
        {
            ShoppingListId = shoppingListId,
            IngredientsToEditQuantity = await _shoppingListIngredientService.GetByShoppingListIdAsync(shoppingListId)
        };
    }
}