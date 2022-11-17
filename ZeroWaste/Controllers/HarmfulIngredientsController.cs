using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroWaste.Data.Services.HarmfulIngredients;

namespace ZeroWaste.Controllers
{
    [Authorize]
    public class HarmfulIngredientsController : Controller
    {
        private readonly IHarmfulIngredientsService _harmfulIngredientsService;
        public HarmfulIngredientsController(IHarmfulIngredientsService harmfulIngredientsService)
        {
            _harmfulIngredientsService = harmfulIngredientsService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var safeIngredients = await _harmfulIngredientsService.GetSafeIngredientsForUser(userId);
            var harmfulIngredients = await _harmfulIngredientsService.GetHarmfulIngredientsForUser(userId);
            ViewBag.SafeIngredients = safeIngredients;
            ViewBag.HarmfulIngredients = harmfulIngredients;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> MarkAsHarmful(int ingredientId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _harmfulIngredientsService.MarkIngredientAsHarmful(userId, ingredientId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> UnmarkAsHarmful(int ingredientId)
        {
            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            await _harmfulIngredientsService.UnmarkIngredientAsHarmful(userId, ingredientId);
            return RedirectToAction(nameof(Index));
        }
    }
}
