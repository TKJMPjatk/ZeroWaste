using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Static;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Services.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly AppDbContext _context;
        private readonly IPhotoMapperHelper _mapperHelper;

        public PhotoService(AppDbContext context, IPhotoMapperHelper mapperHelper)
        {
            _context = context;
            _mapperHelper = mapperHelper;
        }

        public async Task AddRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId)
        {
            var recipe = await _context.Recipes
                .Where(c => c.Id == recipeId)
                .FirstOrDefaultAsync();
            if (recipe is null)
            {
                return;
            }

            foreach (var file in files)
            {
                var photo = new Models.Photo()
                {
                    BinaryPhoto = await file.GetBytes(),
                    RecipeId = recipeId,
                    Name = file.Name,
                    CreatedAt = DateTime.Now,
                };
                await _context.Photos.AddAsync(photo);
            }
            await _context.SaveChangesAsync();
        }

        public async Task AddReviewPhotoAsync(IFormFile file, int? reviewId)
        {
            var review = await _context.RecipeReviews
               .Where(c => c.Id == reviewId)
               .FirstOrDefaultAsync();
            if (review is null)
            {
                return;
            }

            var photo = new Models.Photo()
            {
                BinaryPhoto = await file.GetBytes(),
                RecipeReviewId = reviewId,
                Name = file.Name,
                CreatedAt = DateTime.Now,
            };
            await _context.Photos.AddAsync(photo);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRecipePhotosAsync(IEnumerable<int> photosIds, int recipeId)
        {
            var recipe = await _context.Recipes
                .Where(c => c.Id == recipeId)
                .Include(c => c.Photos)
                .FirstOrDefaultAsync();
            if (recipe is null)
            {
                return;
            }

            var recipePhotosIds = photosIds
                .Where(c => recipe.Photos.Select(c => c.Id).Contains(c));
            foreach (int id in recipePhotosIds)
            {
                var photo = await _context.Photos.FindAsync(id);
                EntityEntry entityEntry = _context.Entry<Models.Photo>(photo);
                entityEntry.State = EntityState.Deleted;
            }
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PhotoVM>> GetPhotoVMAsync(int recipeId)
        {
            var photos = await _context.Photos
                .Where(c => c.RecipeId == recipeId).ToListAsync();
            var mappedPhotos = _mapperHelper.Map(photos);
            return mappedPhotos;
        }
    }
}
