using Microsoft.AspNetCore.Mvc;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.RecipeService;
using ZeroWaste.Data.Services.Reviews;
using ZeroWaste.Data.ViewModels.ExistingRecipe;

namespace ZeroWaste.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly IReviewsService _reviewsService;
        private readonly IPhotoService _photoService;
        private readonly IRecipeService _recipeService;

        public ReviewsController(IReviewsService reviewsService, IPhotoService photoService, IRecipeService recipeService)
        {
            _reviewsService = reviewsService;
            _photoService = photoService;
            _recipeService = recipeService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(DetailsRecipeVM details, IFormFile? filesUpload)
        {
            if (details is null)
            {
                return BadRequest();
            }

            var recipe = await _recipeService.GetByIdAsync(details.NewReviewRecipeId);
            if (recipe is null)
            {
                return NotFound();
            }


            int reviewId = await _reviewsService.AddNewReturnsIdAsync(details.NewReviewRecipeId, details.NewReviewStars, details.NewReviewDescription);
            if (filesUpload is not null)
            {
                await _photoService.AddNewReviewPhotoAsync(filesUpload, reviewId);
            }
            return RedirectToAction("Details","Recipes",new { id = details.NewReviewRecipeId });
        }
    }
}
