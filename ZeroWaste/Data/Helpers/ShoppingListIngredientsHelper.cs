using AutoMapper;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers;

public class ShoppingListIngredientsHelper : IShoppingListIngredientsHelper
{
    private readonly IIngredientsService _ingredientsService;
    private readonly IShoppingListsService _shoppingListsService;
    private readonly IMapper _mapper;
    public ShoppingListIngredientsHelper(IIngredientsService ingredientsService, IShoppingListsService shoppingListsService, IMapper mapper)
    {
        _ingredientsService = ingredientsService;
        _shoppingListsService = shoppingListsService;
        _mapper = mapper;
    }
    public async Task<ShoppingListIngredientsVm> GetShoppingListIngredients(int id)
    {
        ShoppingListIngredientsVm listIngredientsVm = new()
        {
            ShoppingListId = id,
            IngredientsToAddVm = await GetNewIngredientShoppingListVM(id)
        };
        return listIngredientsVm;
    }
    private async Task<List<IngredientsToAddVm>> GetNewIngredientShoppingListVM(int id)
    {
        List<Ingredient> ingredients = await _ingredientsService.GetAllAsync();
        List<IngredientsToAddVm> ingredientShoppingListVms = new();
        ShoppingList? shoppingListWithIngredients = await _shoppingListsService.GetAllIngredientsAsync(id);
        List<int> listOfIds = shoppingListWithIngredients.ShoppingListIngredients.Select(x => x.Ingredient.Id).ToList();
        foreach (var item in ingredients)
        {
            IngredientsToAddVm shoppingListVm = _mapper.Map<IngredientsToAddVm>(item);

            shoppingListVm.IsAdded = listOfIds.Contains(item.Id);
            ingredientShoppingListVms.Add(shoppingListVm);
        }
        return ingredientShoppingListVms
            .OrderBy(x => x.IsAdded)
            .ToList();
    }
}