using AutoMapper;
using ZeroWaste.Data.ViewModels.Ingredients;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Mappers;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Ingredient, NewIngredientShoppingListVM>();
        CreateMap<NewIngredientVM, Ingredient>();
        CreateMap<Ingredient, NewIngredientVM>();
    }
}