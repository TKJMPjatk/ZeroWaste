﻿using ZeroWaste.Data.Static;

namespace ZeroWaste.Data.Services.Photo
{
    public class PhotoService : IPhotoService
    {
        private readonly AppDbContext _context;

        public PhotoService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddNewRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId)
        {
            foreach (var file in files)
            {
                var photo = new Models.Photo()
                {
                    BinaryPhoto = await file.GetBytes(),
                    RecipeId = recipeId
                };
                await _context.Photos.AddAsync(photo);
            }
            await _context.SaveChangesAsync();
        }
    }
}