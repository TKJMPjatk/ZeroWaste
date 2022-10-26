using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Handlers.ShoppingListIngredients;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.RecipeService;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.Services.Reviews;

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
        services.AddScoped<IUrlQueryHelper, UrlQueryHelper>();
        services.AddScoped<IShoppingListIngredientsHelper, ShoppingListIngredientsHelper>();
        services.AddScoped<IIngredientMapperHelper, IngredientMapperHelper>();
        services.AddScoped<IShoppingListHandler, ShoppingListHandler>();
        services.AddScoped<IRecipeMapperHelper, RecipeMapperHelper>();
        services.AddScoped<IRecipeIngredientMapperHelper, RecipeIngredientMapperHelper>();
        services.AddScoped<IShoppingListIngredientsHandler, ShoppingListIngredientsHandler>();
        services.AddScoped<ISearchRecipeHandler, SearchRecipeHandler>();
        services.AddScoped<IAutomatedShoppingListHandler, AutomatedShoppingListHandler>();
    }
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IShoppingListsService, ShoppingListsService>();
        services.AddScoped<IIngredientsService, IngredientsService>();
        services.AddScoped<IShoppingListIngredientsService, ShoppingListIngredientsService>();
        services.AddScoped<IIngredientSelectionService, IngredientSelectionService>();
        services.AddScoped<IRecipesSearchService, RecipesSearchService>();
        services.AddScoped<IRecipeService, RecipeService>();
        services.AddScoped<IRecipesService, RecipesService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();
        services.AddScoped<IReviewsService, ReviewsService>();
    }
}