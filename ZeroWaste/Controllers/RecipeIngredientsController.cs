using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.RecipeService;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.RecipeIngredients;

namespace ZeroWaste.Controllers
{
    public class RecipeIngredientsController : Controller
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly IIngredientsService _ingredientService;
        private readonly IRecipeService _recipeService;

        public RecipeIngredientsController(IRecipeIngredientService service, IIngredientsService ingredientService, IRecipeService recipeService)
        {
            _recipeIngredientService = service;
            _ingredientService = ingredientService;
            _recipeService = recipeService;
        }

        public async Task<IActionResult> Edit(int recipeId, string? message)
        {
            var recipe = await _recipeService.GetByIdAsync(recipeId);
            if (recipe is null)
            {
                return NotFound();
            }

            var recipeIngredients = await _recipeIngredientService.GetCurrentIngredientsAsync(recipeId);
            var recipeIngredientsDropdownsData = await _recipeIngredientService.GetDropdownsValuesAsync();
            ViewBag.Ingredients = recipeIngredientsDropdownsData.Ingredients.ToList();// new SelectList(recipeIngredientsDropdownsData.Ingredients, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(recipeIngredientsDropdownsData.UnitOfMeasures, "Id", "Name");
            ViewBag.RecipeIngredients = recipeIngredients.ToList();
            ViewBag.IngredientTypes = new SelectList(recipeIngredientsDropdownsData.IngredientTypes, "Id", "Name");
            ViewData["message"]= message;
            @ViewData["recipeId"] = recipeId;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(NewRecipeIngredient newRecipeIngredient)
        {
            var errors = ModelState.Values
                                .SelectMany(m => m.Errors)
                                .Where(e => e.ErrorMessage.StartsWith("Nie"))
                                .Select(e => e.ErrorMessage);
            if (errors.Any())
            {
                var message = "Uwaga błędy!\n" + string.Join(" ", errors);
                return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = newRecipeIngredient.RecipeId, message });
            }

            var recipe = await _recipeService.GetByIdAsync(newRecipeIngredient.RecipeId);
            if (recipe is null)
            {
                return NotFound();
            }

            if (newRecipeIngredient.ExistingIngredientId > 0 && newRecipeIngredient.ExistingIngredientQuantity > 0)
            {
                await _recipeIngredientService.AddIngredientAsync(newRecipeIngredient.RecipeId, newRecipeIngredient.ExistingIngredientId, (double)newRecipeIngredient.ExistingIngredientQuantity);
            }
            else if (newRecipeIngredient.NewIngredientName is not null && 
                newRecipeIngredient.NewIngredientUnitOfMeasureId > 0 && 
                newRecipeIngredient.NewIngredientQuantity > 0 && 
                newRecipeIngredient.NewIngredientTypeId > 0)
            {
                var existingIngredient = await _ingredientService.GetVmByNameAsync(newRecipeIngredient.NewIngredientName);
                if (existingIngredient is not null)
                {
                    await _recipeIngredientService.AddIngredientAsync(newRecipeIngredient.RecipeId, existingIngredient.Id, (double)newRecipeIngredient.NewIngredientQuantity);
                }
                else
                {
                    var ingredient = new NewIngredientVM()
                    {
                        Name = newRecipeIngredient.NewIngredientName,
                        Description = "",
                        UnitOfMeasureId = newRecipeIngredient.NewIngredientUnitOfMeasureId,
                        IngredientTypeId = newRecipeIngredient.NewIngredientTypeId
                    };
                    int newIngredientId = await _ingredientService.AddNewReturnsIdAsync(ingredient);
                    await _recipeIngredientService.AddIngredientAsync(newRecipeIngredient.RecipeId, newIngredientId, (double)newRecipeIngredient.NewIngredientQuantity);
                }
            }
            else
            {
                var message = "Uwaga błędy!\nNie rozpoznano rodzaju operacji!";
                return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = newRecipeIngredient.RecipeId, message });
            }
            return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = newRecipeIngredient.RecipeId });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var recipeIngredient = await _recipeIngredientService.GetByIdAsync(id);
            if (recipeIngredient is null)
            {
                return NotFound();
            }
            string message = $"Usunięto składnik: {recipeIngredient.Ingredient.Name}";
            int recipeId = recipeIngredient.RecipeId;
            await _recipeIngredientService.DeleteAsync(id);
            return RedirectToAction("Edit", "RecipeIngredients", new { recipeId, message });
        }
    }
}
