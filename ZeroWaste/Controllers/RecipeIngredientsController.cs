﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ZeroWaste.Data.Services.RecipeIngredients;

namespace ZeroWaste.Controllers
{
    public class RecipeIngredientsController : Controller
    {
        private readonly IRecipeIngredientService _service;

        public RecipeIngredientsController(IRecipeIngredientService service)
        {
            _service = service;
        }

        public async Task<IActionResult> NewRecipeIngredients(int? recipeId)
        {
            var recipeIngredientsDropdownsData = await _service.GetDropdownsValuesAsync();
            ViewBag.Ingredients = new SelectList(recipeIngredientsDropdownsData.Ingredients, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(recipeIngredientsDropdownsData.UnitOfMeasures, "Id", "Name");

            //return View();
            return NotFound();
        }
    }
}