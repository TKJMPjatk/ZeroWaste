using ZeroWaste.Data.ViewModels;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public interface IRecipeMapperHelper
    {
        public Recipe Map(NewRecipeVM newRecipeVM);
    }
}
