using ZeroWaste.Data.ViewModels.ExistingRecipe;
namespace ZeroWaste.Data.Services.Photo
{
    public interface IPhotoService
    {
        Task AddRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId);
        Task AddReviewPhotoAsync(IFormFile file, int? reviewId);
        Task DeleteRecipePhotosAsync(IEnumerable<int> ids, int recipeId);
        Task<IEnumerable<PhotoVM>> GetPhotoVMAsync(int recipeId);
        Task<byte[]> GetFirstByRecipeAsync(int recipeId);
    }
}
