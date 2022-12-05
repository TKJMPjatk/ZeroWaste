using AutoMapper;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public class RecipeIngredientMapperHelper : IRecipeIngredientMapperHelper
    {
        private readonly IMapper _mapper;

        public RecipeIngredientMapperHelper(IMapper mapper)
        {
            _mapper = mapper;
        }


        public IEnumerable<ExistingIngredient> Map(IEnumerable<Ingredient> ingredient)
        {
            IEnumerable<ExistingIngredient> existingIngredient = _mapper.Map<IEnumerable<ExistingIngredient>>(ingredient);
            return existingIngredient;
        }
        public IEnumerable<ExistingIngredient> MapHarmless(IEnumerable<HarmlessIngredient> harmlessIngredients)
        {
            var ingredientsMapped = _mapper.Map<IEnumerable<ExistingIngredient>>(harmlessIngredients);
            return ingredientsMapped;
        }
    }
}
