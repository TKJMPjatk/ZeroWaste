using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.ShoppingLists;

namespace ZeroWaste;

public class ServicesConfiguration
{
    public static ServicesConfiguration GetInstance()
    {
        return new ServicesConfiguration();
    }
    public void Configure(IServiceCollection services)
    {
        ConfigureServices(services);
        services.AddScoped<IShoppingListIngredientService, ShoppingListIngredientService>();
        services.AddScoped<IUrlQueryHelper, UrlQueryHelper>();
        services.AddScoped<IShoppingListIngredientsHelper, ShoppingListIngredientsHelper>();
    }
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IShoppingListsService, ShoppingListsService>();
        services.AddScoped<IIngredientsService, IngredientsService>();
    }
}