using AutoMapper;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public class RecipeMapperHelper : IRecipeMapperHelper
    {
        private readonly IMapper _mapper;

        public RecipeMapperHelper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Recipe Map(NewRecipeVM newRecipeVM)
        {
            Recipe recipe = _mapper.Map<Recipe>(newRecipeVM);
            return recipe;
        }

        public DetailsRecipeVM Map(Recipe recipe)
        {
            DetailsRecipeVM detailsRecipe = _mapper.Map<DetailsRecipeVM>(recipe);
            return detailsRecipe;
        }
    }
}
