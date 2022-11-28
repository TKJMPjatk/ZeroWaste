using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;
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

        public RecipeIngredientsController(IRecipeIngredientService service, IIngredientsService ingredientService, IRecipesService recipeService)
        {
            _recipeIngredientService = service;
            _ingredientService = ingredientService;
            _recipesService = recipeService;
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
                return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = newRecipeIngredient.RecipeId, error = message });
            }
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
    }
}
