using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers.RecipeCategoryImage;

public class CategoryImageProvider : ICategoryImageProvider
{
    private IWebHostEnvironment _hostEnvironment;
    public CategoryImageProvider(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }
    public string GetMatchedImage(Category category)
    {       
        var isFileExist = _hostEnvironment
            .WebRootFileProvider
            .GetFileInfo($"/images/categories/{category.Name}.png")
            .Exists;
        if (isFileExist)
            return $"~/images/categories/{category.Name}.png";
        return $"~/images/categories/Burgery.png";
    }
}