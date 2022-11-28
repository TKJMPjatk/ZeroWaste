using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.RecipeIngredients;

namespace ZeroWaste.Data.Handlers.RecipeIngredientsHandlers
{
    public class RecipeIngredientsHandler : IRecipeIngredientsHandler
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly IIngredientsService _ingredientsService;

        public RecipeIngredientsHandler(IRecipeIngredientService recipeIngredientService, IIngredientsService ingredientsService)
        {
            _recipeIngredientService = recipeIngredientService;
            _ingredientsService = ingredientsService;
        }

        public async Task AddIngredient(NewRecipeIngredient newRecipeIngredient)
        {

            if (newRecipeIngredient.UserFillExistingIngredientGaps())
            {
                await AddIngredientToRecipe(newRecipeIngredient.RecipeId, newRecipeIngredient.ExistingIngredientId, newRecipeIngredient.ExistingIngredientQuantity);
            }
            else if(newRecipeIngredient.UserFillNewIngredientGaps()) 
            {
                await AddNewIngredientToRecipe(newRecipeIngredient);
            }
            else
            {
                throw new ArgumentException("Uwaga błędy!\nNie rozpoznano rodzaju operacji!");
            }
        }
        private async Task AddIngredientToRecipe(int recipeId, int ingredientId, double quantity)
        {
            if (await RecipeIngredientAlredyExists(recipeId, ingredientId))
            {
                await UpdateRecipeIngredientQuantity(recipeId, ingredientId, quantity);
            }
            else
            {
                await AddRecipeIngredientAsync(recipeId, ingredientId, quantity);
            }
        }
        private async Task AddNewIngredientToRecipe(NewRecipeIngredient newRecipeIngredient)
        {
            int ingredientId;
            if (! await IngredientAlredyExists(newRecipeIngredient.NewIngredientName))
            {
                ingredientId = await AddNewIngredient(newRecipeIngredient.NewIngredientName, newRecipeIngredient.ExistingIngredientUnitOfMeasureId, newRecipeIngredient.NewIngredientTypeId);
            }
            else
            {
                var ingredient = await _ingredientsService.GetByNameAsync(newRecipeIngredient.NewIngredientName);
                ingredientId = ingredient.Id;
            }
            await AddIngredientToRecipe(newRecipeIngredient.RecipeId, ingredientId, newRecipeIngredient.NewIngredientQuantity);
        }
        private async Task<int> AddNewIngredient(string newIngredientName, int existingIngredientUnitOfMeasureId, int newIngredientTypeId)
        {
            var ingredient = new NewIngredientVM()
            {
                Name = newIngredientName,
                Description = "",
                UnitOfMeasureId = existingIngredientUnitOfMeasureId,
                IngredientTypeId = newIngredientTypeId
            };
            int newIngredientId = await _ingredientsService.AddNewReturnsIdAsync(ingredient);
            return newIngredientId;
        }
        private async Task AddRecipeIngredientAsync(int recipeId, int ingredientId, double ingredientQuantity)
        {
            await _recipeIngredientService.AddIngredientAsync(recipeId, ingredientId, ingredientQuantity);
        }
        private async Task UpdateRecipeIngredientQuantity(int recipeId, int ingredientId, double ingredientQuantity)
        {
            await _recipeIngredientService.UpdateRecipeIngredientQuantity(recipeId, ingredientId, ingredientQuantity);
        }
        private async Task<bool> RecipeIngredientAlredyExists(int recipeId, int ingredientId)
        {
            bool exists = await _recipeIngredientService.RecipeIngredientAlredyExistsAsync(recipeId, ingredientId);
            return exists;
        }
        private async Task<bool> IngredientAlredyExists(string ingredientName)
        {
            bool exists = await _ingredientsService.IngredientExistsAsync(ingredientName);
            return exists;
        }
    }
}
