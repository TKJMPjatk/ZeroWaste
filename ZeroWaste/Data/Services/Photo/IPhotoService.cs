namespace ZeroWaste.Data.Services.Photo
{
    public interface IPhotoService
    {
        Task AddNewRecipePhotosAsync(IEnumerable<IFormFile> files, int? recipeId);
    }
}
