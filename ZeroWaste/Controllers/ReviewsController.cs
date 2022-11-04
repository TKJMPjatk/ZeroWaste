using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Services.Reviews;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.ExistingRecipe;

namespace ZeroWaste.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;
        private readonly IPhotoService _photoService;
        private readonly IRecipesService _recipeService;
        private readonly IAccountHandler _accountHandler;

        public ReviewsController(IReviewsService reviewsService, IPhotoService photoService, IRecipesService recipeService, IAccountHandler accountHandler)
        {
            _reviewsService = reviewsService;
            _photoService = photoService;
            _recipeService = recipeService;
            _accountHandler = accountHandler;
        }
        [HttpPost]
        public async Task<IActionResult> Create(DetailsRecipeVM details, IFormFile? filesUpload)
        {
            if (details is null)
            {
                return View("BadRequest");
            }

            var recipe = await _recipeService.GetByIdAsync(details.NewReviewRecipeId);
            if (recipe is null)
            {
                return View("NotFound");
            }

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var user = await _accountHandler.GetByIdAsync(userId);
            if (user is null)
            {
                return RedirectToAction("Details", "Recipes", new { id = details.NewReviewRecipeId, error = "Użytkownik nie zalogowany lub nie istnieje"});
            }

            if (await _reviewsService.ReviewExists(details.NewReviewRecipeId, userId))
            {
                return RedirectToAction("Details", "Recipes", new { id = details.NewReviewRecipeId, error = "Nie możesz dodać dwóch opinii na jednym przepisie." });
            }


            int reviewId = await _reviewsService.AddNewReturnsIdAsync(details.NewReviewRecipeId, details.NewReviewStars, details.NewReviewDescription, userId);
            if (filesUpload is not null)
            {
                await _photoService.AddReviewPhotoAsync(filesUpload, reviewId);
            }
            return RedirectToAction("Details","Recipes",new { id = details.NewReviewRecipeId, success = "Pomyślnie dodano opinię" });
        }
    }
}
