using AutoMapper;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Helpers.RecipeCategoryImage;

public class CategorySearchVmMapper : ICategorySearchVmMapper
{
    private readonly ICategoryImageProvider _categoryImageProvider;
    private readonly IMapper _mapper;
    public CategorySearchVmMapper(ICategoryImageProvider categoryImageProvider, IMapper mapper)
    {
        _categoryImageProvider = categoryImageProvider;
        _mapper = mapper;
    }
    public CategorySearchVm MapToCategorySearchVm(Category category)
    {
        var categorySearchVm = _mapper.Map<CategorySearchVm>(category);
        categorySearchVm.Image = _categoryImageProvider.GetMatchedImage(category);
        return categorySearchVm;
    }

    public List<CategorySearchVm> MapToCategorySearchVmsList(List<Category> categories)
    {
        List<CategorySearchVm> categorySearchVmsList = new List<CategorySearchVm>();
        categories
            .ForEach(x => 
                categories.Add(MapToCategorySearchVm(x)));
        return categorySearchVmsList;
    }
}