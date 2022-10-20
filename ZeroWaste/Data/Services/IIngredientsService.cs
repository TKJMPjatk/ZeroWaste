using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services
{
    public interface IIngredientsService
    {
        Task<List<Ingredient>> GetAllAsync();
        Task<List<Ingredient>> GetAllAsync(string searchString);
        Task<NewIngredientDropdownsWM> GetNewIngredientDropdownsWM();
        Task<Ingredient> GetByIdAsync(int? id);
        Task<NewIngredientVM> GetVmByIdAsync(int? id);
        Task<NewIngredientVM> GetVmByNameAsync(string name);
        Task AddNewAsync(NewIngredientVM newIngredient);
        Task<int> AddNewReturnsIdAsync(NewIngredientVM newIngredient);
        Task UpdateAsync(NewIngredientVM updatedIngredient);
        Task DeleteAsync(int? id);
    }
}
