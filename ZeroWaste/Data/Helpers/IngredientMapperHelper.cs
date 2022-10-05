using AutoMapper;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public class IngredientMapperHelper : IIngredientMapperHelper
    {
        private readonly IMapper _mapper;

        public IngredientMapperHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public NewIngredientVM Map(Ingredient ingredient)
        {
            NewIngredientVM ingredientVm = _mapper.Map<NewIngredientVM>(ingredient);
            return ingredientVm;
        }

        public Ingredient Map(NewIngredientVM ingredientVm)
        {
            Ingredient ingredient = _mapper.Map<Ingredient>(ingredientVm);
            return ingredient;
        }
    }
}
