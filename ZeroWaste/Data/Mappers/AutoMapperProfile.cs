using AutoMapper;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Ingredient, IngredientsToAddVm>();
        CreateMap<NewIngredientVM, Ingredient>();
        CreateMap<Ingredient, NewIngredientVM>();
        CreateMap<NewShoppingListVM, ShoppingList>();
        CreateMap<Category, CategorySearchVm>();
        CreateMap<NewRecipeVM, Recipe>();
    }
}