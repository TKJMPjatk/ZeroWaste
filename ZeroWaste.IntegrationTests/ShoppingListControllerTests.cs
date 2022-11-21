using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
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
        NewShoppingListVM shoppingListVm = new NewShoppingListVM()
        {
            Note = "Note",
            Title = "Title"
        };
        var httpContent = shoppingListVm.ToJsonHttpContent();
        var response = await _client.PostAsync("/ShoppingLists/Create", httpContent);
        var absolutPath = response.RequestMessage.RequestUri.AbsolutePath;
        var statusCode = response.StatusCode;
        Assert.Equal("/ShoppingListIngredients/IngredientsToAdd/1", absolutPath);
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }
    [Fact]
    public async Task Create_ForInvalidViewModel_ShouldReturnStatusCodeOkAndPathToCreateMethod()
    {
        NewShoppingListVM shoppingListVm = new NewShoppingListVM();
        var httpContent = shoppingListVm.ToJsonHttpContent();
        var response = await _client.PostAsync("/ShoppingLists/Create", httpContent);
        var absolutPath = response.RequestMessage.RequestUri.AbsolutePath;
        var statusCode = response.StatusCode;
        Assert.Equal("/ShoppingLists/Create", absolutPath);
        Assert.Equal(HttpStatusCode.OK, statusCode);
    }
    [Fact]
    public async Task Index_Always_ShouldReturnStatusCodeOk()
    {       
        ShoppingList shoppingList = new ShoppingList()
        {
            Id = 1,
            Note = "TestNote",
            Title = "TestTitle",
            UserId = "1"
        };
        SeedShoppingList(shoppingList);
        var response = await _client.GetAsync("/ShoppingLists");
        var textResult = await response.Content.ReadAsStringAsync();
        var isExists = textResult.Contains(shoppingList.Title) && textResult.Contains(shoppingList.Note);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.True(isExists);
    }
    private void SeedShoppingList(ShoppingList shoppingList)
    {
 
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        _dbContext.ShoppingLists.Add(shoppingList);
        _dbContext.SaveChanges();
    }
}