﻿using ZeroWaste.Data.ViewModels.ExistingRecipe;

namespace ZeroWaste.Data.Services.Photo
{
    public interface IPhotoService
    {
<<<<<<< HEAD
        Task AddRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId);
        Task AddReviewPhotoAsync(IFormFile file, int? reviewId);
        Task DeleteRecipePhotosAsync(IEnumerable<int> ids, int recipeId);
        Task<IEnumerable<PhotoVM>> GetPhotoVMAsync(int recipeId);
=======
        Task AddNewRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId);
        Task AddNewReviewPhotoAsync(IFormFile file, int? reviewId);
        Task<byte[]> GetFirstByRecipeAsync(int recipeId);
>>>>>>> TK
    }
}
