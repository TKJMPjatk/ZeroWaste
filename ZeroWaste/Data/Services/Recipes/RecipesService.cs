using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
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

        public async Task<int> AddNewReturnsIdAsync(NewRecipeVM newRecipeVM, string userId)
        {

            Recipe recipe = _mapperHelper.Map(newRecipeVM);
            recipe.AuthorId = userId;
            recipe.Status = await _context.Statuses.Where(c => c.Name == "Niepotwierdzony")
                .FirstOrDefaultAsync();
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return recipe.Id;
        }

        public async Task<Recipe?> GetByIdAsync(int id)
        {
            var entity = await _context
            .Recipes
            .AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Id == id);
            return entity;
        }

        public async Task<EditRecipeVM?> GetEditByIdAsync(int id)
        {
            var entity = await _context
               .Recipes
               .AsNoTracking()
               .Include(d => d.Photos)
               .FirstOrDefaultAsync(x =>
                   x.Id == id);
            var mappedEntity = _mapperHelper.MapToEdit(entity);
            return mappedEntity;

        }

        public async Task<DetailsRecipeVM?> GetDetailsByIdAsync(int id)
        {
            var recipe = await _context.Recipes
                .AsNoTracking()
                .Where(c => c.Id == id)
                .Include(c => c.Photos)
                .Include(c => c.Category)
                .Include(c => c.RecipesIngredients)
                .ThenInclude(c => c.Ingredient)
                .ThenInclude(c => c.UnitOfMeasure)
                .Include(c => c.RecipeReviews)
                .ThenInclude(c => c.Photos)
                .Include(c => c.RecipeReviews)
                .ThenInclude(c => c.ApplicationUser)
                .FirstOrDefaultAsync();
            var detailsRecipe = _mapperHelper.MapToDetails(recipe);
            return detailsRecipe;
        }

        public async Task<RecipeDropdownVM> GetDropdownsValuesAsync()
        {
            var response = new RecipeDropdownVM()
            {
                Categories = await _context.Categories.ToListAsync(),
            };
            return response;
        }

        public async Task UpdateAsync(EditRecipeVM editRecipeVM, string userId)
        {
            var recipe = _mapperHelper.MapFromEdit(editRecipeVM);
            recipe.AuthorId = userId;
            recipe.Status = await _context.Statuses.Where(c => c.Name == "Niepotwierdzony").FirstOrDefaultAsync();
            _context.Update(recipe);
            await _context.SaveChangesAsync();
        }

        public async Task AddLiked(int recipeId, string userId)
        {
            var likedRecipe = await _context.FavouriteRecipes
                .FirstOrDefaultAsync(c => c.RecipeId == recipeId && c.UserId == userId);
            if (likedRecipe is null)
            {
                var newLikedRecipe = new FavouriteRecipe()
                {
                    RecipeId = recipeId,
                    UserId = userId
                };
                await _context.FavouriteRecipes.AddAsync(newLikedRecipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task AddNotLiked(int recipeId, string userId)
        {
            var hatedRecipe = await _context.HatedRecipes
                .FirstOrDefaultAsync(c => c.RecipeId == recipeId && c.UserId == userId);
            if (hatedRecipe is null)
            {
                var newHatedRecipe = new HatedRecipe()
                {
                    RecipeId = recipeId,
                    UserId = userId
                };
                await _context.HatedRecipes.AddAsync(newHatedRecipe);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> IsAuthorEqualsEditor(int recipeId, string editorId)
        {
            var entity = await _context
           .Recipes
           .AsNoTracking()
           .FirstOrDefaultAsync(x =>
               x.Id == recipeId);
            if (entity is null || Equals(entity.AuthorId, editorId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<List<int>> GetRecipeIdList()
        {
            return await _context
                .Recipes
                .Where(x => x.StatusId == 1)
                .Select(x => x.Id)
                .ToListAsync();
        }

        public async Task ConfirmRecipe(int recipeId)
        {
            var entity = await _context
                .Recipes
                .FirstOrDefaultAsync(x => x.Id == recipeId);
            var confirmStatusId = await _context
                .Statuses
                .Where(x => x.Name == "Zatwierdzony")
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            entity.StatusId = confirmStatusId;
            EntityEntry entityEntry = _context.Entry(entity);
            entityEntry.State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStateAsync(int recipeId, int statusId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            recipe.StatusId = statusId;
            _context.Entry(recipe).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
