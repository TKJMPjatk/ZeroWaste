using ZeroWaste.Data.Handlers.Account;
using ZeroWaste.Data.Handlers.AdminSearchHadlers;
using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.Handlers.SearchRecipesHandlers;
using ZeroWaste.Data.Handlers.SearchRecipeStrategy;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Handlers.ShoppingListIngredients;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Helpers.RecipeCategoryImage;
using ZeroWaste.Data.Helpers.SearchByIngredients;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.FavouriteRecipes;
using ZeroWaste.Data.Services.HatedRecipes;
using ZeroWaste.Data.Services.RecipesSearch;
using ZeroWaste.Data.Services.Photo;
using ZeroWaste.Data.Services.RecipeIngredients;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.Services.Reviews;
using ZeroWaste.Data.Services.Statuses;

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
        ConfigureStrategy(services);
        ConfigureHelpers(services);
        services.AddScoped<IUrlQueryHelper, UrlQueryHelper>();
        services.AddScoped<IShoppingListIngredientsHelper, ShoppingListIngredientsHelper>();
        services.AddScoped<IIngredientMapperHelper, IngredientMapperHelper>();
        services.AddScoped<IShoppingListHandler, ShoppingListHandler>();
        services.AddScoped<IRecipeMapperHelper, RecipeMapperHelper>();
        services.AddScoped<IRecipeIngredientMapperHelper, RecipeIngredientMapperHelper>();
        services.AddScoped<IPhotoMapperHelper, PhotoMapperHelper>();
        services.AddScoped<IShoppingListIngredientsHandler, ShoppingListIngredientsHandler>();
        services.AddScoped<ISearchRecipeHandler, SearchRecipeHandler>();
        services.AddScoped<IAutomatedShoppingListHandler, AutomatedShoppingListHandler>();
        services.AddScoped<IAccountHandler, AccountHandler>();
        services.AddScoped<IAdminSearchHandler, AdminSearchHandler>();
    }

    private void ConfigureHelpers(IServiceCollection services)
    {
        services.AddScoped<ICategoryImageProvider, CategoryImageProvider>();
        services.AddScoped<ICategorySearchVmMapper, CategorySearchVmMapper>();
        services.AddScoped<ISearchByIngredientAdder, SearchByIngredientAdder>();
    }
    private void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IShoppingListsService, ShoppingListsService>();
        services.AddScoped<IIngredientsService, IngredientsService>();
        services.AddScoped<IShoppingListIngredientsService, ShoppingListIngredientsService>();
        services.AddScoped<IIngredientSelectionService, IngredientSelectionService>();
        services.AddScoped<IRecipesSearchService, RecipesSearchService>();
        services.AddScoped<IRecipesService, RecipesService>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<IRecipeIngredientService, RecipeIngredientService>();
        services.AddScoped<IStatusesService, StatusesService>();
        services.AddScoped<IFavouritesRecipesService, FavouritesRecipesService>();
        services.AddScoped<IHatedRecipesService, HatedRecipesService>();
    }    
    private void ConfigureStrategy(IServiceCollection services)
    {
        services.AddScoped<ISearchRecipeContext, SearchRecipeContext>();
        services.AddScoped<IReviewsService, ReviewsService>();
    }
}