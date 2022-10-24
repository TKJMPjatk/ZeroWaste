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
        var recipeDropdownsData = await _recipesService.GetDropdownsValuesAsync();
        ViewBag.Categories = new SelectList(recipeDropdownsData.Categories, "Id", "Name");
        return View();
    }

    public async Task<IActionResult> Details(int id)
    {
        var recipeDetails = await _recipesService.GetDetailsByIdAsync(id);
        if (recipeDetails is null)
        {
            return NotFound();
        }
        return View(recipeDetails);
    }
    [HttpPost]
    public async Task<IActionResult> Create(NewRecipeVM recipeVM, IEnumerable<IFormFile> filesUpload)
    {
        if (!ModelState.IsValid)
        {
            var recipeDropdownsData = await _recipesService.GetDropdownsValuesAsync();
            ViewBag.Categories = new SelectList(recipeDropdownsData.Categories, "Id", "Name");
            return View(recipeVM);
        }
        int recipeId = await _recipesService.AddNewReturnsIdAsync(recipeVM);
        await _photoService.AddNewRecipePhotosAsync(filesUpload, recipeId);
        
        return RedirectToAction("Edit", "RecipeIngredients", new { recipeId });
    }
}