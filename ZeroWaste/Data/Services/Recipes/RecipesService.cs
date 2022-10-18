﻿using Microsoft.EntityFrameworkCore;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.NewRecepie;
using ZeroWaste.Data.ViewModels.RecipeIngredients;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Recipes
{
    public class RecipesService : IRecipesService
    {
        private readonly AppDbContext _context;
        private readonly IRecipeMapperHelper _mapperHelper;

        public RecipesService(AppDbContext context, IRecipeMapperHelper mapperHelper)
        {
            _context = context;
            _mapperHelper = mapperHelper;
        }

        public async Task AddNewAsync(NewRecipeVM newRecipeVM)
        {
            Recipe recipe = _mapperHelper.Map(newRecipeVM);
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task<int> AddNewReturnsIdAsync(NewRecipeVM newRecipeVM)
        {
            Recipe recipe = _mapperHelper.Map(newRecipeVM);
            // TODO : Only for tests - remove!
            var ILLEGAL_CODE_TO_REMOVE = AppDbInitializer.userIds[2];
            recipe.AuthorId = ILLEGAL_CODE_TO_REMOVE;
            recipe.Status = await _context.Statuses.Where(c => c.Name == "Niepotwierdzony").FirstOrDefaultAsync();
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe.Id;
        }

        public Task<Recipe> GetByIdAsync(int? id)
        {
            throw new NotImplementedException();
        }

        public async Task<RecipeDropdownVM> GetDropdownsValuesAsync()
        {
            var response = new RecipeDropdownVM()
            {
                Categories = await _context.Categories.ToListAsync(),
            };
            return response;
        }
    }
}