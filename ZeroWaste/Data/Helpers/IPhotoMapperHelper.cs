using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers
{
    public interface IPhotoMapperHelper
    {
        IEnumerable<PhotoVM> Map(IEnumerable<Photo> photos);
    }
}
