namespace ZeroWaste.Data.Services.Photo
{
    public interface IPhotoService
    {
        Task AddNewRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId);
        Task AddNewReviewPhotoAsync(IFormFile file, int? reviewId);
        Task<byte[]> GetFirstByRecipeAsync(int recipeId);
    }
}
