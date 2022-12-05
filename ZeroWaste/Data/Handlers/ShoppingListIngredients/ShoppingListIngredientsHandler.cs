using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.IngredientsTypes;
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
    private readonly IIngredientsTypesService _ingredientsTypesService ;
    public ShoppingListIngredientsHandler(IShoppingListIngredientsService shoppingListIngredientService, IIngredientsService ingredientsService, IShoppingListsService shoppingListsService, IIngredientsTypesService ingredientsTypesService)
    {
        _shoppingListIngredientService = shoppingListIngredientService;
        _ingredientsService = ingredientsService;
        _shoppingListsService = shoppingListsService;
        _ingredientsTypesService = ingredientsTypesService;
    }
    public async Task<ShoppingListIngredientsVm> GetShoppingListIngredientsVm(int shoppingListId, string searchString, int? typeId)
    {
        List<IngredientsToAddVm> ingredientsList;
        if (typeId is null || typeId == 0)
        {
            ingredientsList = (await _ingredientsService.GetIngredientsToAddVmAsync(shoppingListId, searchString))
                .Where(x =>x .IsAdded == false).ToList();
        }
        else
        {
            ingredientsList = (await _ingredientsService.GetIngredientsToAddVmAsync(shoppingListId, searchString))
                .Where(x => x.IngredientTypeId == typeId && x.IsAdded == false)
                .ToList();
        }
        ShoppingListIngredientsVm shoppingListIngredientsVm = new ShoppingListIngredientsVm()
        {
            ShoppingListId = shoppingListId,
            IngredientsToAddVm = await _ingredientsService.GetAddedIngredientsAsync(shoppingListId),
            IngredientTypes = await _ingredientsTypesService.GetAllAsync(),
            Ingredients = ingredientsList,
            SelectedCategoryId = typeId is null ? 0 : (int)typeId
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

    public async Task DeleteIngredientFromShoppingList(int ingredientId, int shoppingListId)
    {
        await _shoppingListIngredientService.DeleteAsync(shoppingListId, ingredientId);
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