using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers.RecipeCategoryImage;

public interface ICategoryImageProvider
{
    string GetMatchedImage(Category category);
}