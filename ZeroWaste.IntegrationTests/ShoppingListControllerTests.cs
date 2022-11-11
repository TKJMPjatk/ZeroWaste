using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using ZeroWaste.Data;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests;

public class ShoppingListControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;
    private HttpClient _client;
    public ShoppingListControllerTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOpotions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<AppDbContext>));
                    services.Remove(dbContextOpotions);
                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDB"));
                });
            });
        _client = _factory.CreateClient();
    }
    private AppDbContext GetAppDbContext()
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        return dbContext;
    }

    private void SeedShoppingList(ShoppingList shoppingList)
    {
        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        AppDbContext dbContext = scope.ServiceProvider.GetService<AppDbContext>();
        dbContext.ShoppingLists.Add(shoppingList);
        dbContext.SaveChanges();
    }
    [Fact]
    public async Task EditTitle_ForExistingShoppingList_ShouldReturn()
    {
        ShoppingList shoppingList = new ShoppingList()
        {
            Title = "Test1",
            Note = "Note",
            CreatedAt = System.DateTime.Now,
            UserId = Guid.NewGuid().ToString()
        };
        SeedShoppingList(shoppingList);
        var httpContent = shoppingList.ToJsonHttpContent();
        var response = await _client.PostAsync($"/ShoppingLists/EditTitle", httpContent);
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
