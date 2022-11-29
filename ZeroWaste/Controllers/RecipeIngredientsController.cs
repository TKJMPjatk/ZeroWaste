using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroWaste.Data.Handlers.RecipeIngredientsHandlers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using AuthorizeAttribute = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;
using Controller = Microsoft.AspNetCore.Mvc.Controller;
using SelectList = Microsoft.AspNetCore.Mvc.Rendering.SelectList;

namespace ZeroWaste.Controllers
{
    [Authorize]
    public class RecipeIngredientsController : Controller
    {
        private readonly IRecipeIngredientService _recipeIngredientService;
        private readonly IIngredientsService _ingredientService;
        private readonly IRecipesService _recipesService;
        private readonly IRecipeIngredientsHandler _recipeIngredientsHandler;

        public RecipeIngredientsController(IRecipeIngredientService service, IIngredientsService ingredientService, IRecipesService recipeService, IRecipeIngredientsHandler recipeIngredientsHandler)
        {
            _recipeIngredientService = service;
            _ingredientService = ingredientService;
            _recipesService = recipeService;
            _recipeIngredientsHandler = recipeIngredientsHandler;
        }

        public async Task<IActionResult> Edit(int recipeId, string? error, string? success)
        {
            var recipe = await _recipesService.GetByIdAsync(recipeId);
            if (recipe is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!await _recipesService.IsAuthorEqualsEditor(recipeId, userId))
            {
                Response.StatusCode = 401;
                return View("Unauthorized");
            }

            var recipeIngredients = await _recipeIngredientService.GetCurrentIngredientsAsync(recipeId);
            var recipeIngredientsDropdownsData = await _recipeIngredientService.GetDropdownsValuesAsync();
            ViewBag.Ingredients = recipeIngredientsDropdownsData.Ingredients.ToList();
            ViewBag.UnitOfMeasures = new SelectList(recipeIngredientsDropdownsData.UnitOfMeasures, "Id", "Name");
            ViewBag.RecipeIngredients = recipeIngredients.ToList();
            ViewBag.IngredientTypes = new SelectList(recipeIngredientsDropdownsData.IngredientTypes, "Id", "Name");
            ViewData["Error"]= error;
            ViewData["Success"] = success;
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

            var recipe = await _recipesService.GetByIdAsync(newRecipeIngredient.RecipeId);
            if (recipe is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!await _recipesService.IsAuthorEqualsEditor(newRecipeIngredient.RecipeId, userId))
            {
                Response.StatusCode = 401;
                return View("Unauthorized");
            }

            try
            {
                await _recipeIngredientsHandler.AddIngredient(newRecipeIngredient);
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = newRecipeIngredient.RecipeId, error = ex.Message });
            }

            await _recipesService.UnconfirmRecipe(newRecipeIngredient.RecipeId);

            return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = newRecipeIngredient.RecipeId, success = "Pomyślnie dodano składnik" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var recipeIngredient = await _recipeIngredientService.GetByIdAsync(id);
            if (recipeIngredient is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            int recipeId = recipeIngredient.RecipeId;
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (!await _recipesService.IsAuthorEqualsEditor(recipeId, userId))
            {
                Response.StatusCode = 401;
                return View("Unauthorized");
            }

            string success = $"Usunięto składnik: {recipeIngredient.Ingredient.Name}";
            await _recipeIngredientService.DeleteAsync(id);
            return RedirectToAction("Edit", "RecipeIngredients", new { recipeId, success });
        }

        public IActionResult FakeSaveRedirect()
        {
            string success = "Zmiany w przepisie i składnikach zostały zapisane pomyślnie!";
            return RedirectToAction("SearchMineToEdit", "SearchRecipes", new { success });
        }
    }
}
