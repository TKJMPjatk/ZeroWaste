using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.IngredientsTypes;

public interface IIngredientsTypesService
{
     Task<List<IngredientType>> GetAllAsync();
}