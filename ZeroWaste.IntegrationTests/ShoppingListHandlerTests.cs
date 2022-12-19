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
    private WebApplicationFactory<Program> _factory;
    IShoppingListHandler _shoppingListHandler;
    private AppDbContext _dbContext;
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
        //var con = _dbContext.Database.GetConnectionString();
    }
    [Fact]
    public async Task GetShoppingListById_forExistingShoppingList_ShouldReturnShoppingList()
    {
        //Arrange
        var existedShoppingList = await SeedShoppingList(_dbContext);
        //Act
        var shoppingList = await _shoppingListHandler.GetShoppingListById(existedShoppingList.Id);
        //Assert
        Assert.Equal(existedShoppingList.Id, shoppingList.Id);
        CleanDb.Clean(_connectionString);
    }
    private async Task<ShoppingList> SeedShoppingList(AppDbContext context)
    {
        var shopppingList = new ShoppingList()
        {
            Note = Guid.NewGuid().ToString(),
            Title = Guid.NewGuid().ToString(),
            CreatedAt = System.DateTime.Now,
            UserId = "0d442e09-d1a4-43ea-b1da-a60a7cd0c6a0"
        };
        await context.ShoppingLists.AddAsync(shopppingList);
        await context.SaveChangesAsync();
        return shopppingList;
    }
    [Fact]
    public async Task GetShoppingListByUserId_ForExistingShoppingLists_ShouldReturnShoppingList()
    {
        //Arrange
        var userId = await GetUserGuid();
        var existedShoppingList = await SeedShoppingList(_dbContext, userId);
        //Act 
        var shoppingLists = await _shoppingListHandler.GetShoppingListsByUserId(userId);
        //Assert
        Assert.Equal(2, shoppingLists.Count());
        CleanDb.Clean(_connectionString);
    }
    private async Task<string> GetUserGuid()
    {
        var userGuid = await _dbContext.Users.Select(x => x.Id).FirstOrDefaultAsync();
        return userGuid;
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
        await context.ShoppingLists.AddRangeAsync(shopppingList1);
        await context.SaveChangesAsync();
        return shoppingLists;
    }
    [Fact]
    public async Task Create_ForValidModel_ShouldAddNewShoppingList()
    {
        //Arrange
        var userId = await GetUserGuid();
        var shoppingListVm = new NewShoppingListVM()
        {
            Note = Guid.NewGuid().ToString(),
            Title = Guid.NewGuid().ToString()
        };
        //Act
        var shoppingLists = await _shoppingListHandler.Create(shoppingListVm ,userId);
        //Assert
        Assert.Equal(1, await IsShoppingListExists(shoppingListVm));
        CleanDb.Clean(_connectionString);
    }
    private async Task<int> IsShoppingListExists(NewShoppingListVM shoppingListVm)
    {
        return await _dbContext
            .ShoppingLists
            .CountAsync(x =>
                x.Title == shoppingListVm.Title 
            &&  x.Note == shoppingListVm.Note);
    }

}