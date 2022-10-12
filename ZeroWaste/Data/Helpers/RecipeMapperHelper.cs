using AutoMapper;
using ZeroWaste.Data.ViewModels;
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

        public Recipe Map(this NewRecipeVM newRecipeVM)
        {
            Recipe recipe = _mapper.Map<Recipe>(newRecipeVM);
            return recipe;
        }
    }
}
