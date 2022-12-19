using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.IntegrationTests.Helpers;

namespace ZeroWaste.IntegrationTests;

public class ShoppingListHandlerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly string _connectionString;
    private WebApplicationFactory<Program> _factory;
    IShoppingListHandler shoppingListsService;
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
    }

    [Fact]
    public async Task Test()
    {
        var ss = _factory.Services.CreateScope();
        var h = ss.ServiceProvider.GetService<IShoppingListHandler>();
        var test = await h.GetShoppingListById(2);
    }

    [Fact]
    public async Task Testing()
    {
        var tmp = _connectionString;
        var tmp2 = shoppingListsService;
        Assert.Equal(_connectionString, _connectionString);
    }
}