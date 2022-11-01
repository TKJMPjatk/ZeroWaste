﻿using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Data.ViewModels.NewRecepie;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Recipes
{
    public interface IRecipesService
    {
        Task AddNewAsync(NewRecipeVM newRecipeVM);
        Task<int> AddNewReturnsIdAsync(NewRecipeVM newRecipeVM);
        Task<Recipe> GetByIdAsync(int id);
        Task<RecipeDropdownVM> GetDropdownsValuesAsync();
        Task<DetailsRecipeVM> GetDetailsByIdAsync(int id);
        Task<EditRecipeVM> GetEditByIdAsync(int id);
        Task UpdateAsync(EditRecipeVM editRecipeVM);
        Task AddLiked(int recipeId);
        Task AddNotLiked(int recipeId);
    }
}
