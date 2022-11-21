using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public interface IRecipeMapperHelper
    {
        public Recipe Map(NewRecipeVM newRecipeVM);
        public DetailsRecipeVM MapToDetails(Recipe? recipe);
        public EditRecipeVM MapToEdit(Recipe? recipe);
        public Recipe MapFromEdit (EditRecipeVM editRecipeVM);
    }
}
