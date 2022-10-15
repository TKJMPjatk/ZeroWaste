using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> AddIngredients(int? recipeId)
        {
            var recipeDropdownsData = await _service.GetDropdownsValuesAsync();
            ViewBag.Ingredients = new SelectList(recipeDropdownsData.Ingredients, "Id", "Name");
            ViewBag.UnitOfMeasures = new SelectList(recipeDropdownsData.UnitOfMeasures, "Id", "Name");

            //return View();
            return NotFound();
        }
    }
}
