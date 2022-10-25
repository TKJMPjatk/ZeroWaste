using AutoMapper;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;
using ZeroWaste.Data.ViewModels.RecipeIngredients;

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
        CreateMap<UnitOfMeasure, ExistingUnitOfMeasure>();
        CreateMap<Ingredient, ExistingIngredient>();
        CreateMap<Recipe, RecipeResult>()
         .ForMember(dest => dest.Ingredients,
             opt => opt.MapFrom(
                 src => src.RecipesIngredients
                     .Select(x => x.Ingredient.Name)
                     .ToList()));

    }
}