using ZeroWaste.Data.ViewModels.NewRecepie;

namespace ZeroWaste.Data.Services.Photo
{
    public interface IPhotoService
    {
        Task AddNewAsync(FileVM file, int? recipeId);
    }
}
