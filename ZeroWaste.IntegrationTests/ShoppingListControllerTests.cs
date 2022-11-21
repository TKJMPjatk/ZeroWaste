using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data;
using ZeroWaste.Data.ViewModels.ShoppingList;
using ZeroWaste.IntegrationTests.Helpers;

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
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.Remove(dbContextOptions);
                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDb"));
                });
            });
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