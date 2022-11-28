using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers
{
    [Authorize]
    public class IngredientsController : Controller
    {
        private readonly IIngredientsService _ingredientsService;
        private readonly IRecipeIngredientService _recipeIngredientsService;
        public IngredientsController(IIngredientsService ingredientsService, IRecipeIngredientService recipeIngredientsService)
        {
            _ingredientsService = ingredientsService;
            _recipeIngredientsService = recipeIngredientsService;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(string searchString)
        {
            List<Ingredient> ingredients;
            if (!string.IsNullOrEmpty(searchString))
            {
                ingredients = await _ingredientsService.GetAllAsync(searchString);
            }
            else
            {
                ingredients = await _ingredientsService.GetAllAsync();
            }
            return View(ingredients);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            var ingredientDetails = await _ingredientsService.GetByIdAsync(id);
            if (ingredientDetails is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            return View(nameof(Details),ingredientDetails);
        }

        public async Task<IActionResult> Create()
        {
            var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
            ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

            return View(nameof(Create));
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IngredientTypeId,UnitOfMeasureId")] NewIngredientVM ingredient)
        {
            if (!ModelState.IsValid)
            {
                var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
                ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
                ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

                return View(nameof(Create),ingredient);
            }
            await _ingredientsService.AddNewAsync(ingredient);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var ingredientDetail = await _ingredientsService.GetVmByIdAsync(id);
            if (ingredientDetail is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
            ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

            return View(nameof(Edit), ingredientDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewIngredientVM ingredient)
        {
            if (id != ingredient.Id)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }

            if (!ModelState.IsValid)
            {
                var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
                ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
                ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");
                return View(nameof(Edit), ingredient);
            }

            await _ingredientsService.UpdateAsync(ingredient);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id, string? message)
        {
            var ingredientDetails = await _ingredientsService.GetByIdAsync(id);
            if (ingredientDetails is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            ViewData["Error"] = message;
            return View(nameof(Delete),ingredientDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientDetails = await _ingredientsService.GetByIdAsync(id);
            if (ingredientDetails is null)
            {
                Response.StatusCode = 404;
                return View("NotFound");
            }
            if (await _recipeIngredientsService.RecipeIngredientsExisting(id))
            {
                return RedirectToAction("Delete", "Ingredients", new { id, message = "Składnik jest powiązany z przepisem! Nie można go usunąć." });
            }

            await _ingredientsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
