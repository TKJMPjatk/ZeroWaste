using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ZeroWaste.Data;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests;

public class ShoppingListHandlerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string _connectionString;
    private WebApplicationFactory<Program>? _factory;
    private readonly IShoppingListHandler? _shoppingListHandler;
    private readonly AppDbContext? _dbContext;
    private readonly ShoppingListHandlerHelper _shoppingListHandlerHelper;
    public ShoppingListHandlerTests(WebApplicationFactory<Program> factory)
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
                });
            });
        var serviceScope = _factory.Services.CreateScope();
        _shoppingListHandler = serviceScope.ServiceProvider.GetService<IShoppingListHandler>();
        _dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        _shoppingListHandlerHelper = new ShoppingListHandlerHelper();
    }
    [Fact]
    public async Task GetShoppingListById_forExistingShoppingList_ShouldReturnShoppingList()
    {
        //Arrange
        var existedShoppingList = await _shoppingListHandlerHelper.SeedShoppingList(_dbContext);
        //Act
        var shoppingList = await _shoppingListHandler.GetShoppingListById(existedShoppingList.Id);
        //Assert
        Assert.Equal(existedShoppingList.Id, shoppingList.Id);
        CleanDb.Clean(_connectionString);
    }

    [Fact]
    public async Task GetShoppingListByUserId_ForExistingShoppingLists_ShouldReturnShoppingList()
    {
        CleanDb.Clean(_connectionString);
        //Arrange
        var userId = await _shoppingListHandlerHelper.GetUserGuid(_dbContext);
        var existedShoppingList = await SeedShoppingList(_dbContext, userId);
        //Act 
        var shoppingLists = await _shoppingListHandler.GetShoppingListsByUserId(userId);
        //Assert
        Assert.Equal(existedShoppingList.Count, shoppingLists.Count());
        CleanDb.Clean(_connectionString);
    }

    private async Task<List<ShoppingList>> SeedShoppingList(AppDbContext context, string userId)
    {
        var shopppingList = new ShoppingList()
        {
            Note = Guid.NewGuid().ToString(),
            Title = Guid.NewGuid().ToString(),
            CreatedAt = System.DateTime.Now,
            UserId = userId
        };
        await context.ShoppingLists.AddAsync(shopppingList);
        await context.SaveChangesAsync();
        var shopppingList1 = new ShoppingList()
        {
            Note = Guid.NewGuid().ToString(),
            Title = Guid.NewGuid().ToString(),
            CreatedAt = System.DateTime.Now,
            UserId = userId
        };
        List<ShoppingList> shoppingLists = new List<ShoppingList>()
        {
            shopppingList,
            shopppingList1
        };
        await context.ShoppingLists.AddAsync(shopppingList1);
        await context.SaveChangesAsync();
        return shoppingLists;
    }
    [Fact]
    public async Task Create_ForValidModel_ShouldAddNewShoppingList()
    {
        //Arrange
        var userId = await _shoppingListHandlerHelper.GetUserGuid(_dbContext);
        var shoppingListVm = new NewShoppingListVM()
        {
            Note = Guid.NewGuid().ToString(),
            Title = Guid.NewGuid().ToString()
        };
        //Act
        var shoppingLists = await _shoppingListHandler.Create(shoppingListVm ,userId);
        //Assert
        Assert.Equal(1, await _shoppingListHandlerHelper.GetShoppingListsCount(shoppingListVm, _dbContext));
        CleanDb.Clean(_connectionString);
    }
    [Fact]
    public async Task Delete_ForExistingShoppingList_ShouldDeleteShoppingList()
    {
        //Arrange
        var shoppingList = await _shoppingListHandlerHelper.SeedShoppingList(_dbContext); 
        //Act
        await _shoppingListHandler.DeleteAsync(shoppingList.Id);
        //Assert
        Assert.Equal(0, await _shoppingListHandlerHelper.GetShoppingListsCount(shoppingList.Id, _dbContext));
        CleanDb.Clean(_connectionString);
    }

    [Fact]
    public async Task IsShoppingListExists_ForExistingShoppingList_ShouldReturnTrue()
    {
        //Arrange
        var shoppingList = await _shoppingListHandlerHelper.SeedShoppingList(_dbContext); 
        //Act
        var result = await _shoppingListHandler.IsShoppingListExists(shoppingList.Id);
        //Assert
        Assert.True(result);
        CleanDb.Clean(_connectionString);
    }
}