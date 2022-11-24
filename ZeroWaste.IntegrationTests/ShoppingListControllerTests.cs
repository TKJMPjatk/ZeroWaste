using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using NuGet.Protocol;
using ZeroWaste.Data;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests;

public class ShoppingListControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    public ShoppingListControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                    services.Remove(dbContextOptions);
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDb"));
                });
            });
        _client = _factory.CreateClient();
    }
    [Fact]
    public async Task Create_ForValidViewModel_ShouldReturnStatusCodeOkAndPathToIngredientsToAddWithId()
    {
        //Arrange
        NewShoppingListVM shoppingListVm = GetNewShoppingListVm(true);
        var httpContent = shoppingListVm.ToJsonHttpContent();
        //Act
        var response = await _client.PostAsync("/ShoppingLists/Create", httpContent);
        //Assert
        int id = await GetId(shoppingListVm.Title, shoppingListVm.Note);
        Assert.Equal($"/ShoppingListIngredients/IngredientsToAdd/{id}", response.RequestMessage.RequestUri.AbsolutePath);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Create_ForInvalidViewModel_ShouldReturnStatusCodeOkAndPathToCreateMethod()
    {
        //Arrange
        NewShoppingListVM shoppingListVm = GetNewShoppingListVm(false);
        var httpContent = shoppingListVm.ToJsonHttpContent();
        //Act
        var response = await _client.PostAsync("/ShoppingLists/Create", httpContent);
        //Assert
        Assert.Equal("/ShoppingLists/Create", response.RequestMessage.RequestUri.AbsolutePath);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Index_Always_ShouldReturnStatusCodeOk()
    {
        //Arrange
        ShoppingList shoppingList = GetShoppingList();
        await AddShoppingListToDatabase(shoppingList);
        //Act
        var response = await _client.GetAsync("/ShoppingLists");
        //Assert
        var textResult = await response.Content.ReadAsStringAsync();
        var isExists = textResult.Contains(shoppingList.Title) && textResult.Contains(shoppingList.Note);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(isExists);
    }
    [Fact]
    public async Task Edit_ForExistingShoppingList_ShouldReturnStatusCodeOk()
    {
        //Arrange
        ShoppingList shoppingList = GetShoppingList();
        int id = await AddShoppingListToDatabase(shoppingList);
        //Act
        var response = await _client.GetAsync($"/ShoppingLists/Edit/{id}");
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }    
    [Fact]
    public async Task Edit_ForNonExistingShoppingList_ShouldReturnStatusCodeNotFound()
    {
        //Arrange
        ShoppingList shoppingList = GetShoppingList();
        int id = await AddShoppingListToDatabase(shoppingList);
        //Act
        var response = await _client.GetAsync($"/ShoppingLists/Edit/{id+1}");
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task Delete_ForExistingShoppingList_ShouldReturnStatusCodeOk()
    {
        //Arrange
        ShoppingList shoppingList = GetShoppingList();
        int id = await AddShoppingListToDatabase(shoppingList);
        //Act
        var response = await _client.GetAsync($"/ShoppingLists/Delete/{id}");
        //Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task Delete_ForNonExistingShoppingList_ShouldReturnStatusCodeNotFound()
    {
        //Arrange
        ShoppingList shoppingList = GetShoppingList();
        int id = await AddShoppingListToDatabase(shoppingList);
        //Act
        var response = await _client.GetAsync($"/ShoppingLists/Delete/{id+1}");
        //Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
    [Fact]
    public async Task EditTitle_ForExistingShoppingList_ShouldReturnStatusCode()
    {        
        //Arrange
        ShoppingList shoppingList = GetShoppingList();
        int id = await AddShoppingListToDatabase(shoppingList);
        shoppingList.Id = id;
        var httpContent = shoppingList.ToJsonHttpContent();
        //Act
        var response = await _client.PostAsync("/ShoppingLists/EditTitle", httpContent); 
        //Assert
        var textResult = await response.Content.ReadAsStringAsync();
        var isExists = textResult.Contains(shoppingList.Title);
        Assert.True(isExists);
        Assert.Equal($"/ShoppingLists/Edit/{id}", response.RequestMessage.RequestUri.AbsolutePath);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    private NewShoppingListVM GetNewShoppingListVm(bool correct)
    {
        if(correct)
        {
            return new NewShoppingListVM()
            {
                Title = Guid.NewGuid().ToString(),
                Note = Guid.NewGuid().ToString()
            };
        }
        else
        {
            return new NewShoppingListVM();
        }
    }

    private ShoppingList GetShoppingList()
    {
        return new ShoppingList()
        {
            Title = Guid.NewGuid().ToString(),
            Note = Guid.NewGuid().ToString(),
            UserId = "1"
        };
    }
    private async Task<int> AddShoppingListToDatabase(ShoppingList shoppingList)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        await _dbContext.ShoppingLists.AddAsync(shoppingList);
        await _dbContext.SaveChangesAsync();
        return shoppingList.Id;
    }
    private async Task<int> GetId(string title, string note)
    {        
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        int id = await _dbContext.ShoppingLists.Where(x => x.Title == title && x.Note == note).Select(x => x.Id).FirstOrDefaultAsync();
        return id;
    }
}