using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers
{
    public class IngredientsController : Controller
    {
        private readonly IIngredientsService _ingredientsService;

        public IngredientsController(IIngredientsService ingredientsService)
        {
            _ingredientsService = ingredientsService;
        }

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

        public async Task<IActionResult> Details(int? id)
        {
            var ingredientDetails = await _ingredientsService.GetByIdAsync(id);
            if (ingredientDetails is null)
            {
                return NotFound();
            }
            return View(ingredientDetails);
        }

        public async Task<IActionResult> Create()
        {
            var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
            ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,IngredientTypeId,UnitOfMeasureId")] NewIngredientVM ingredient)
        {
            if (!ModelState.IsValid)
            {
                var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
                ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
                ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

                return View(ingredient);
            }
<<<<<<< HEAD

            await _ingredientsService.AddNewAsync(ingredient);
            return RedirectToAction(nameof(Index));
=======
            ViewData["IngredientTypeId"] = new SelectList(_context.IngredientTypes, "Id", "Id", ingredient.IngredientTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", ingredient.UnitOfMeasureId);
            return View();
>>>>>>> fe03965 (Added service for ingredients in shopping list and added configuration class to DI)
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var ingredientDetail = await _ingredientsService.GetVmByIdAsync(id);
            if (ingredientDetail is null)
            {
                return NotFound();
            }

<<<<<<< HEAD
            var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
            ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");

            return View(ingredientDetail);
=======
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
            {
                return NotFound();
            }
            ViewData["IngredientTypeId"] = new SelectList(_context.IngredientTypes, "Id", "Id", ingredient.IngredientTypeId);
            ViewData["UnitOfMeasureId"] = new SelectList(_context.UnitOfMeasures, "Id", "Id", ingredient.UnitOfMeasureId);
            return View();
>>>>>>> fe03965 (Added service for ingredients in shopping list and added configuration class to DI)
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NewIngredientVM ingredient)
        {
            if (id != ingredient.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                var ingredientDropdownsData = await _ingredientsService.GetNewIngredientDropdownsWM();
                ViewBag.IngredientTypes = new SelectList(ingredientDropdownsData.IngredientTypes, "Id", "Name");
                ViewBag.UnitOfMeasures = new SelectList(ingredientDropdownsData.UnitOfMeasures, "Id", "Name");
                return View(ingredient);
            }

            await _ingredientsService.UpdateAsync(ingredient);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var ingredientDetails = await _ingredientsService.GetByIdAsync(id);
            if (ingredientDetails is null)
            {
                return NotFound();
            }
            return View(ingredientDetails);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ingredientDetails = await _ingredientsService.GetByIdAsync(id);
            if (ingredientDetails is null)
            {
                return NotFound();
            }
            await _ingredientsService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
