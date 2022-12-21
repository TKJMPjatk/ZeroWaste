using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Handlers.AutomatedShoppingList;
using ZeroWaste.Data.Handlers.ShoppingListIngredients;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests;

[Collection("#1")]
public class AutomatedShoppingListHandlerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string _connectionString;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly AppDbContext _dbContext;
    private readonly IAutomatedShoppingListHandler _automatedShoppingListHandler;
    public AutomatedShoppingListHandlerTests(WebApplicationFactory<Program> factory)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        _connectionString = configuration.GetConnectionString("TestConnectionString");
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                    services.Remove(dbContextOptions);
                    services.AddDbContext<AppDbContext>(options => options.UseSqlServer(_connectionString));
                    var IDbConnection = services.Where(service =>
                        service.ServiceType == typeof(IDbConnectionFactory)
                    ).ToList();
                    foreach (var item in IDbConnection)
                    {
                        services.Remove(item);
                    }
                    services.AddScoped<IDbConnectionFactory, TestDbConnectionFactory>();
                });
            });
        var serviceScope = _factory.Services.CreateScope();
        _dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        _automatedShoppingListHandler = serviceScope.ServiceProvider.GetService<IAutomatedShoppingListHandler>();
    }

    [Fact]
    public async Task AddNewShoppingList_ForRecipeIdAndUserId_ShouldAddShoppingList()
    {
        //Arrange
        var ingredient1 = await SeedIngredient();
        var ingredient2 = await SeedIngredient();
        var recipe = await SeedRecipe();
        var recipeIngredient1 = await SeedIngredientToRecipe(recipe, ingredient1);
        var recipeIngredient2 = await SeedIngredientToRecipe(recipe, ingredient2);
        var user = await _dbContext.Users.FirstOrDefaultAsync();
        //Act
        await _automatedShoppingListHandler.AddNewShoppingList(recipe.Id, user.Id);
        //Asser
        var countShoppingList = await _dbContext.ShoppingLists.CountAsync();
        var countShoppingListIngredient = await _dbContext.ShoppingListIngredients
            .Where(x => x.IngredientId == ingredient1.Id || x.IngredientId == ingredient2.Id).CountAsync();
        Assert.Equal(1, countShoppingList);
        Assert.Equal(2, countShoppingListIngredient);
        await CleanDb.Clean(_connectionString);
    }

    private async Task<Recipe> SeedRecipe()
    {
        var author = await _dbContext.Users.FirstOrDefaultAsync();
        var category = await _dbContext.Categories.FirstOrDefaultAsync();
        var recipe = new Recipe() 
        {
            Title = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
            CategoryId = 1, 
            DifficultyLevel = 1, 
            EstimatedTime = 1, 
            AuthorId = author.Id, 
            StatusId = await _dbContext.Statuses.Where(c => c.Name == "Zatwierdzony").Select(c => c.Id).FirstOrDefaultAsync()
        };
        await _dbContext.AddAsync(recipe);
        await _dbContext.SaveChangesAsync();
        _dbContext.Entry(recipe).State = EntityState.Detached;
        return recipe;
    }
    private async Task<Ingredient> SeedIngredient()
    {
        var unitOfMeasure =  await _dbContext.UnitOfMeasures.FirstOrDefaultAsync();
        var ingredientType =  await _dbContext.IngredientTypes.FirstOrDefaultAsync(); 
        Ingredient newIngredient = new Ingredient()
        {
            Name = Guid.NewGuid().ToString(), 
            Description = Guid.NewGuid().ToString(), 
            UnitOfMeasureId = unitOfMeasure.Id, 
            IngredientTypeId = ingredientType.Id
        }; 
        await _dbContext.Ingredients.AddAsync(newIngredient); 
        await _dbContext.SaveChangesAsync(); 
        return newIngredient;
    }
    private async Task<RecipeIngredient> SeedIngredientToRecipe(Recipe recipe, Ingredient ingredient)
    {
        RecipeIngredient recipeIngredient1 = new RecipeIngredient()
        {
            IngredientId = ingredient.Id,
            RecipeId = recipe.Id,
            Quantity = 0,
        };
        await _dbContext.RecipeIngredients.AddAsync(recipeIngredient1);
        await _dbContext.SaveChangesAsync();
        return recipeIngredient1;
    }
}