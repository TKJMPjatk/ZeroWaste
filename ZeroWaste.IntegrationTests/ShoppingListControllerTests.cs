using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.IntegrationTests.Helpers;

namespace ZeroWaste.IntegrationTests;

public class ShoppingListControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private HttpClient _client;
    private WebApplicationFactory<Program> _factory;
    public ShoppingListControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task Create_Tests()
    {
        NewShoppingListVM shoppingListVm = new NewShoppingListVM()
        {
            Note = "Note",
            Title = "Title"
        };
        var httpContent = shoppingListVm.ToJsonHttpContent();
        var respone = await _client.PostAsync("https://localhost:7227/ShoppingLists/Create", httpContent);
    }
}