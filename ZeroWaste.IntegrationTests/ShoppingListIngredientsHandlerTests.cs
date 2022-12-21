using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.CodeAnalysis.Differencing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ZeroWaste.Data;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Handlers.ShoppingListIngredients;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests;
[Collection("#1")]
public class ShoppingListIngredientsHandlerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string _connectionString;
    private readonly WebApplicationFactory<Program> _factory;
    private readonly IShoppingListIngredientsHandler _shoppingListIngredientsHandler;
    private readonly AppDbContext? _dbContext;
    private static readonly object SyncObject = new object();
    public ShoppingListIngredientsHandlerTests(WebApplicationFactory<Program> factory)
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
                    //services.Remove(dbDbConnection);
                    services.AddScoped<IDbConnectionFactory, TestDbConnectionFactory>();
                });
            });
        var serviceScope = _factory.Services.CreateScope();
        _dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        _shoppingListIngredientsHandler = serviceScope.ServiceProvider.GetService<IShoppingListIngredientsHandler>();
    }
    [Fact]
    public async Task EditQuantityOfNewIngredient_ForNewIngredient_ShouldChangeQuantity()
    {
        //Arrange
        var newShoppingListIngredient = await SeedShopingListWithIngredients();
        newShoppingListIngredient.Quantity = 10;
        EditQuantityVM editQuantityVm = new EditQuantityVM()
        {
            ShoppingListId = newShoppingListIngredient.Id,
            IngredientsToEditQuantity = new List<ShoppingListIngredient>()
            {
                newShoppingListIngredient
            }
        };
        //Act
        await _shoppingListIngredientsHandler.EditQuantityOfNewIngredients(editQuantityVm);
        //Assert
        var shoppingListIngredient = await _dbContext.ShoppingListIngredients
            .Where(x => x.ShoppingListId == editQuantityVm.ShoppingListId)
            .FirstOrDefaultAsync();
        Assert.Equal(10, shoppingListIngredient.Quantity);
        await CleanDb.Clean(_connectionString);
    }

    [Fact]
    public async Task
        ChangeShoppingListIngredientSelection_ForExistingShoppingListIngredient_ShouldChangeIngredientSelection()
    {
        //Arrange 
        var newShoppingListIngredient = await SeedShopingListWithIngredients();
        var selection = newShoppingListIngredient.Selected;
        //Act
        await _shoppingListIngredientsHandler.ChangeShoppingListIngredientSelection(newShoppingListIngredient.Id);
        //Assert
        var itemAfter = await _dbContext.ShoppingListIngredients
            .Where(x => x.Id == newShoppingListIngredient.Id)
            .Select(x => x.Selected)
            .FirstOrDefaultAsync();
        Assert.Equal(!selection, itemAfter);
        await CleanDb.Clean(_connectionString);
    }

    [Fact]
    public async Task HandleDeleteIngredientFromShoppingList_ForShoppingListIngredientId_ShouldDelete()
    {
        //Arrange
        var newShoppingListIngredient = await SeedShopingListWithIngredients();
        var countItemBefore = await _dbContext.ShoppingListIngredients.CountAsync(x => x.Id == newShoppingListIngredient.Id);
        //Act
        await _shoppingListIngredientsHandler.HandleDeleteIngredientFromShoppingList(newShoppingListIngredient.Id);
        //Assert
        var countItemAfter = await _dbContext.ShoppingListIngredients.CountAsync(x => x.Id == newShoppingListIngredient.Id);
        Assert.Equal(1, countItemBefore); 
        Assert.Equal(0, countItemAfter);
        await CleanDb.Clean(_connectionString);
    }

    [Fact]
    public async Task AddIngredientToShoppingList_ForShoppingListIdAndIngredientId_ShouldAddToShoppingListIngredient()
    {
        //Arrange
        var ingredient = await SeedIngredient();
        var shoppingList = await SeedShoppingList();
        //Act
         await _shoppingListIngredientsHandler.AddIngredientToShoppingList(ingredient.Id, shoppingList.Id);
        //Assert
        var count =  _dbContext.ShoppingListIngredients.Count(x =>
            x.ShoppingListId == shoppingList.Id && x.IngredientId == ingredient.Id);
        Assert.Equal(1, count);
        await CleanDb.Clean(_connectionString);
    }
    private async Task<ShoppingListIngredient> SeedShopingListWithIngredients()
    {
        ShoppingList newShoppingList = await SeedShoppingList();
        Ingredient newIngredient = await SeedIngredient();
        ShoppingListIngredient newShoppingListIngredient = new ShoppingListIngredient()
        {
            IngredientId = newIngredient.Id,
            ShoppingListId = newShoppingList.Id,
            Quantity = 0,
            Selected = false
        };
        await _dbContext.ShoppingListIngredients.AddAsync(newShoppingListIngredient);
        await _dbContext.SaveChangesAsync();
        return newShoppingListIngredient;
    }

    private async Task<ShoppingList> SeedShoppingList()
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(); 
        ShoppingList newShoppingList = new ShoppingList() 
        {
            Title = Guid.NewGuid().ToString(), 
            Note = Guid.NewGuid().ToString(), 
            UserId = user.Id, 
            CreatedAt = DateTime.Now
        }; 
        _dbContext.ShoppingLists.Add(newShoppingList); 
        _dbContext.SaveChanges();
        return newShoppingList;
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
}