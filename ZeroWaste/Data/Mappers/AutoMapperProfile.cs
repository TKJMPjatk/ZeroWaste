using AutoMapper;
using ZeroWaste.Data.Structs;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.Data.ViewModels.ShoppingListIngredients;
using ZeroWaste.Models;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Data.ViewModels.ExistingRecipe;

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
        CreateMap<Recipe, DetailsRecipeVM>();
        CreateMap<Recipe, RecipeResult>()
         .ForMember(dest => dest.Ingredients,
             opt => opt.MapFrom(
                 src => src.RecipesIngredients
                     .Select(x => x.Ingredient.Name)
                     .ToList()));
        CreateMap<Recipe, EditRecipeVM>();
        CreateMap<EditRecipeVM, Recipe>();
        CreateMap<Photo, PhotoVM>();
        CreateMap<HarmlessIngredient, ExistingIngredient>()
            .ForMember(d => d.Id, o => o.MapFrom(s => s.IngredientId))
            .ForMember(d => d.Name, o => o.MapFrom(s => s.IngredientName))
            .ForMember(d => d.UnitOfMeasure, o => o.MapFrom(s => new ExistingUnitOfMeasure()
                                                            {
                                                                Id = s.UnitOfMeasureId,
                                                                Name = s.UnitOfMeasureName,
                                                                Shortcut = s.UnitOfMeasureShortcut
                                                            }));
    }
}