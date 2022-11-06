using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers.RecipeCategoryImage;

public interface ICategorySearchVmMapper
{
    CategorySearchVm MapToCategorySearchVm(Category category);
    List<CategorySearchVm> MapToCategorySearchVmsList(List<Category> categories);
}