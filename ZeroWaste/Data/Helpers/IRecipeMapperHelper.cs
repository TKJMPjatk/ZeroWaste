using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public interface IRecipeMapperHelper
    {
        public Recipe Map(NewRecipeVM newRecipeVM);
        public DetailsRecipeVM Map(Recipe recipe);
    }
}
