using AutoMapper;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.ViewModels.CategorySearch;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Handlers.SearchRecipesHandlers;

public class SearchRecipeHandler : ISearchRecipeHandler
{
    private readonly ICategoryService _categoryService;
    private readonly IMapper _mapper;
    private IWebHostEnvironment _hostEnvironment;
    public SearchRecipeHandler(ICategoryService categoryService, IMapper mapper, IWebHostEnvironment hostEnvironment)
    {
        _categoryService = categoryService;
        _mapper = mapper;
        _hostEnvironment = hostEnvironment;
    }
    public async Task<List<CategorySearchVm>> GetCategoriesSearchVm()
    {
        List<Category> categories = await _categoryService.GetAllAsync();
        return mapToCategoriesSearchVms(categories);
    }

    private List<CategorySearchVm> mapToCategoriesSearchVms(List<Category> categories)
    {
        List<CategorySearchVm> categorySearchVmList = new List<CategorySearchVm>();
        foreach (var entity in categories)
        {
            var categorySearchVm = _mapper.Map<CategorySearchVm>(entity);
            categorySearchVm.Image = GetMatchedImage(entity);
            categorySearchVmList.Add(categorySearchVm);
        }
        return categorySearchVmList;
    }

    private string GetMatchedImage(Category category)
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