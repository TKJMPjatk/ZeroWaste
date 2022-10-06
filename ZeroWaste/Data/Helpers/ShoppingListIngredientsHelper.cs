using AutoMapper;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.Ingredients;
using ZeroWaste.Data.ViewModels.ShoppingList;
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
    public async Task<ShoppingListIngredientsVM> GetShoppingListIngredients(int id)
    {
        ShoppingListIngredientsVM listIngredientsVm = new ShoppingListIngredientsVM()
        {
            ShoppingListId = id
        };
        listIngredientsVm.IngredientShoppingListVms = await GetNewIngredientShoppingListVM(id);
        return listIngredientsVm;
    }
    private async Task<List<NewIngredientShoppingListVM>> GetNewIngredientShoppingListVM(int id)
    {
        List<Ingredient> ingredients = await _ingredientsService.GetAllAsync();
        List<NewIngredientShoppingListVM> ingredientShoppingListVms = new List<NewIngredientShoppingListVM>();
        ShoppingList shoppingListWithIngredients = await _shoppingListsService.GetAllIngredientsAsync(id);
        List<int> listOfIds = shoppingListWithIngredients.ShoppingListIngredients.Select(x => x.Ingredient.Id).ToList();
        foreach (var item in ingredients)
        {
            NewIngredientShoppingListVM shoppingListVm = _mapper.Map<NewIngredientShoppingListVM>(item);

            shoppingListVm.IsAdded = listOfIds.Contains(item.Id);
            ingredientShoppingListVms.Add(shoppingListVm);
        }
        return ingredientShoppingListVms
            .OrderBy(x => x.IsAdded)
            .ToList();
    }
}