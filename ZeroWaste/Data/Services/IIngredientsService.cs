using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services
{
    public interface IIngredientsService
    {
        Task<List<Ingredient>> GetAllAsync();
        Task<List<Ingredient>> GetAllAsync(string searchString);
        Task<NewIngredientDropdownsWM> GetNewIngredientDropdownsWM();
    }
}
