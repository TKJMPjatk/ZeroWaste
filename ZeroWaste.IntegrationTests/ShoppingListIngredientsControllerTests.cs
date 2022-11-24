using System.Net;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Razor.Language;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.IntegrationTests.Helpers.Tests;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests;

public class ShoppingListIngredientsControllerTests: IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;

    public ShoppingListIngredientsControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContext = services.SingleOrDefault(services =>
                        services.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                    services.Remove(dbContext);
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.AddDbContext<AppDbContext>(option => option.UseInMemoryDatabase("TestDb"));
                });
            });
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions()
        {
            AllowAutoRedirect = false
        });
    }
    [Fact]
    public async Task DeleteIngredientFromShoppingList_ForExistingShoppingListId_ShouldReturnStatusCodeOk()
    {
        //Arrange
        var shoppingListIngredient = GetShoppingListIngredient();
        shoppingListIngredient = await AddShoppingListIngredientToDatabase(shoppingListIngredient);
        //Act
        var response =
            await _client.GetAsync(
                $"/ShoppingListIngredients/DeleteIngredientFromShoppingList/{shoppingListIngredient.Id}");
        //Assert
        Assert.Equal($"/ShoppingLists/Edit/{shoppingListIngredient.ShoppingListId}", response.RequestMessage.RequestUri.AbsolutePath);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task AddIngredientToShoppingList_ForExistingIngredientAndShoppingList_ShouldReturnStatusCodeOk()
    {
        //Arrange
        var shoppingList = await AddShoppingListToDatabase();
        var ingredient = await AddIngredientToDatabase();
        //Act
        var response = await _client.GetAsync($"/ShoppingListIngredients/AddIngredientToShoppingList/?id={ingredient.Id}&shoppingListId={shoppingList.Id}");
        //Assert
        Assert.Equal($"/ShoppingListIngredients/IngredientsToAdd/{shoppingList.Id}", response.RequestMessage.RequestUri.AbsolutePath);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task EditQuantity_ForExistingShoppingListId_ReturnStatusCodeOk()
    {
        //Arrange
        var shoppingList = await AddShoppingListToDatabase();
        //Act
        var response = await _client.GetAsync($"/ShoppingListIngredients/EditQuantity/{shoppingList.Id}");
        //Assert
        Assert.Equal($"/ShoppingListIngredients/EditQuantity/{shoppingList.Id}", response.RequestMessage.RequestUri.AbsolutePath);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    [Fact]
    public async Task EditQuantity_ForValidEditQuantity_ShouldReturnStatusCodeOk()
    {
        await AddShoppingListIngredientToDatabase(GetShoppingListIngredient());
        var defaultPage = await _client.GetAsync($"/ShoppingListIngredients/EditQuantity/{1}");
        var content = await HtmlHelpers.GetDocumentAsync(defaultPage);
        var tmp = content.QuerySelectorAll("input");
        var response = await _client.SendAsync(
            (IHtmlFormElement) content.QuerySelector("form[id='test1']"),
            (IHtmlButtonElement) content.QuerySelector("button[id='test2']")
        );
        /*
        //Arrange
        var shoppingList = await AddShoppingListToDatabase();
        EditQuantityVM quantityVm = new EditQuantityVM()
        {
            ShoppingListId = shoppingList.Id,
            IngredientsToEditQuantity = new List<ShoppingListIngredient>()
            {
                await AddShoppingListIngredientToDatabase(GetShoppingListIngredient())
            }
        };
        var httpContent = quantityVm.ToJsonHttpContent();
        //Act
        var response = await _client.PostAsync($"/ShoppingListIngredients/EditQuantity/{shoppingList.Id}", httpContent);
        */
    }
    private ShoppingListIngredient GetShoppingListIngredient()
    {
        return new ShoppingListIngredient()
        {
            Quantity = 0,
            Selected = false
        };
    }
    private async Task<ShoppingListIngredient> AddShoppingListIngredientToDatabase(ShoppingListIngredient shoppingListIngredient)
    {
        var shoppingList = await AddShoppingListToDatabase();
        var ingredient = await AddIngredientToDatabase();
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        shoppingListIngredient.IngredientId = ingredient.Id;
        shoppingListIngredient.ShoppingListId = shoppingList.Id;
        await _dbContext.ShoppingListIngredients.AddAsync(shoppingListIngredient);
        await _dbContext.SaveChangesAsync();
        return shoppingListIngredient;
    }

    private async Task<ShoppingList> AddShoppingListToDatabase()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        ShoppingList shoppingList = new ShoppingList()
        {
            Title = Guid.NewGuid().ToString(),
            Note = Guid.NewGuid().ToString(),
            UserId = "1"
        };
        await _dbContext.ShoppingLists.AddAsync(shoppingList);
        await _dbContext.SaveChangesAsync();
        return shoppingList;
    }
    private async Task<Ingredient> AddIngredientToDatabase()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        Ingredient ingredient = new Ingredient()
        {
            Name = Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString(),
        };
        await _dbContext.Ingredients.AddAsync(ingredient);
        await _dbContext.SaveChangesAsync();
        return ingredient;
    }
}