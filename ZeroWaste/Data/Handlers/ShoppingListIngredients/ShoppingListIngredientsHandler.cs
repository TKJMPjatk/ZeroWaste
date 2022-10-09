using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.ShoppingListIngredients;

public class ShoppingListIngredientsHandler : IShoppingListIngredientsHandler
{
    private readonly IShoppingListIngredientsService _shoppingListIngredientService;
    private readonly IIngredientsService _ingredientsService;
    private readonly IMapper _mapper;
    private List<int> _idOfIngredientsInShoppingList = new List<int>();

    public ShoppingListIngredientsHandler(IShoppingListIngredientsService shoppingListIngredientService, IIngredientsService ingredientsService, IMapper mapper)
    {
        _shoppingListIngredientService = shoppingListIngredientService;
        _ingredientsService = ingredientsService;
        _mapper = mapper;
    }
    public async Task<ShoppingListIngredientsVm> GetShoppingListIngredientsVm(int shoppingListId, string searchString)
    {
        await SetShoppingListIngredientIdsList(shoppingListId);
        ShoppingListIngredientsVm shoppingListIngredientsVm = new ShoppingListIngredientsVm()
        {
            ShoppingListId = shoppingListId,
            IngredientsToAddVm = (await GetIngredientsToAddVm(searchString))
                                        .OrderBy(x=>x.IsAdded)
                                        .ToList()
        };
        return shoppingListIngredientsVm;
    }
    private async Task SetShoppingListIngredientIdsList(int shoppingListId)
    {
        _idOfIngredientsInShoppingList = (await _shoppingListIngredientService.GetIngredientsForShoppingList(shoppingListId))
            .Select(x => x.Ingredient.Id)
            .ToList();
    }
    private async Task<List<IngredientsToAddVm>> GetIngredientsToAddVm(string searchString)
    {
        var entities = await _ingredientsService.GetAllAsync();
        var ingredientsToAddVmList = MapIngredientsListToIngredientsToAddVmList(entities);
        return FilterIngredientsToAddVmList(searchString, ingredientsToAddVmList);
    }
    private List<IngredientsToAddVm> MapIngredientsListToIngredientsToAddVmList(List<Ingredient> ingredients)
    {
        List<IngredientsToAddVm> ingredientsToAddVmList = new List<IngredientsToAddVm>();
        foreach (var item in ingredients)
        {
            var newIngredientsToAddVm = _mapper.Map<IngredientsToAddVm>(item);
            newIngredientsToAddVm.IsAdded = IsIngredientAdded(item.Id);
            ingredientsToAddVmList.Add(newIngredientsToAddVm);
        }
        return ingredientsToAddVmList;
    }
    private bool IsIngredientAdded(int ingredientId)
    {
        return _idOfIngredientsInShoppingList.Contains(ingredientId);
    }
    private List<IngredientsToAddVm> FilterIngredientsToAddVmList(string searchString, List<IngredientsToAddVm> ingredientsToAddVms)
    {
        if (string.IsNullOrEmpty(searchString))
            return ingredientsToAddVms;
        return ingredientsToAddVms
                .Where(x => 
                    x.Name.ToLower().Contains(searchString.ToLower()))
                .ToList();
    }
    public async Task EditQuantityOfNewIngredients(EditQuantityVM editQuantityVm)
    {
        foreach (var item in editQuantityVm.IngredientsToEditQuantity)
        {
            await _shoppingListIngredientService
                .EditQuantityAsync(item.Id, item.Quantity);
        }
    }
}