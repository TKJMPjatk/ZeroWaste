using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.Http.Headers;
using System.Security.Claims;
using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Data.ViewModels.Login;
using ZeroWaste.Data.ViewModels.Recipes;
using ZeroWaste.Models;

namespace ZeroWaste.Controllers;

[Authorize]
public class RecipesController : Controller
{
    private readonly IRecipesService _recipesService;
    private readonly IPhotoService _photoService;
    private readonly IAccountHandler _accountHandler;
        
    public RecipesController(IRecipesService recipesService, IPhotoService photoService, IAccountHandler accountHandler)
    {
        _recipesService = recipesService;
        _photoService = photoService;   
        _accountHandler = accountHandler;
    }
    public async Task<IActionResult> Create()
    {
        var recipeDropdownsData = await _recipesService.GetDropdownsValuesAsync();
        ViewBag.Categories = new SelectList(recipeDropdownsData.Categories, "Id", "Name");
        return View();
    }
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id, string? error)
    {
        var recipeDetails = await _recipesService.GetDetailsByIdAsync(id);
        if (recipeDetails is null)
        {
            return View("NotFound");
        }
        ViewData["recipeId"] = id;
        TempData["Error"] = error;
        recipeDetails.NewReviewRecipeId = id;
        return View(recipeDetails);
    }
    public async Task<IActionResult> Edit(int id)
    {
        var recipeDetails = await _recipesService.GetEditByIdAsync(id);
        if (recipeDetails is null)
        {
            return View("NotFound");
        }
        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (!await _recipesService.IsAuthorEqualsEditor(id, userId))
        {
            return Unauthorized();
        }

        var recipeDropdownsData = await _recipesService.GetDropdownsValuesAsync();
        ViewBag.Categories = new SelectList(recipeDropdownsData.Categories, "Id", "Name");
        return View(recipeDetails);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(EditRecipeVM recipe, IEnumerable<IFormFile> filesUpload)
    {
        ModelState["Photos"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
        ModelState["NewPhotosNamesToSkip"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
        ModelState["PhotosToDelete"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
        if (!ModelState.IsValid)
        {
            var recipeDropdownsData = await _recipesService.GetDropdownsValuesAsync();
            var recipePhotos = await _photoService.GetPhotoVMAsync(recipe.Id);
            recipe.Photos = recipePhotos.ToList();
            ViewBag.Categories = new SelectList(recipeDropdownsData.Categories, "Id", "Name");
            return View(recipe);
        }

        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _recipesService.UpdateAsync(recipe, userId);
        if (!string.IsNullOrEmpty(recipe.PhotosToDelete))
        {
            var photosToDelete = recipe.PhotosToDelete
                .Split('|')
                .Where(c => c.Length > 0)
                .Select(c => Convert.ToInt32(c));
            await _photoService.DeleteRecipePhotosAsync(photosToDelete, recipe.Id);
        }
        var newPhotos = filesUpload;
        if (!string.IsNullOrEmpty(recipe.NewPhotosNamesToSkip))
        {
            var newPhotosToSkip = recipe.NewPhotosNamesToSkip.Split('|');
            newPhotos = filesUpload.Where(c => !newPhotosToSkip.Contains(c.FileName));
        }
        await _photoService.AddRecipePhotosAsync(newPhotos, recipe.Id);
        return RedirectToAction("Edit", "RecipeIngredients", new { recipeId = recipe.Id, error = "", success = "Przepis zaktualizowano pomyœlnie." });
    }
    [HttpPost]
    public async Task<IActionResult> Create(NewRecipeVM recipeVM, IEnumerable<IFormFile> filesUpload)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        if (!ModelState.IsValid)
        {
            var recipeDropdownsData = await _recipesService.GetDropdownsValuesAsync();
            ViewBag.Categories = new SelectList(recipeDropdownsData.Categories, "Id", "Name");
            return View(recipeVM);
        }
        int recipeId = await _recipesService.AddNewReturnsIdAsync(recipeVM, userId);
        await _photoService.AddRecipePhotosAsync(filesUpload, recipeId);
        return RedirectToAction("Edit", "RecipeIngredients", new { recipeId });
    }
    [HttpPost]
    public async Task<IActionResult> AddLiked(int recipeId)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _recipesService.AddLiked(recipeId, userId);
        return RedirectToAction("Details", "Recipes", new { id = recipeId });
    }
    [HttpPost]
    public async Task<IActionResult> AddNotLiked(int recipeId)
    {
        string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
        await _recipesService.AddNotLiked(recipeId, userId);
        return RedirectToAction("Details", "Recipes", new { id = recipeId });
    }
}