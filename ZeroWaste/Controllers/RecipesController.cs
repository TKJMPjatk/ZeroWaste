using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.Recipes;

namespace ZeroWaste.Controllers;

public class RecipesController : Controller
{
    private readonly IRecipesService _recipesService;
    private readonly IPhotoService _photoService;
        
    public RecipesController(IRecipesService recipesService, IPhotoService photoService)
    {
        _recipesService = recipesService;
        _photoService = photoService;   
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<IActionResult> Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(NewRecipeVM recipeVM, IEnumerable<IFormFile> filesUpload)
    {
        if (!ModelState.IsValid)
        {
            return View(recipeVM);
        }
        int recipeId = await _recipesService.AddNewReturnsIdAsync(recipeVM);
        await _photoService.AddNewRecipePhotosAsync(filesUpload, recipeId);
        
        return RedirectToAction("AddIngredients", "RecipeIngredientsController", new { recipeId = recipeId });
    }
}