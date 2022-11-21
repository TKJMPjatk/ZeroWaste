using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroWaste.Data;
using ZeroWaste.IntegrationTests.Helpers;

namespace ZeroWaste.IntegrationTests
{
    public class RecipesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public RecipesControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var dbContextOptions = services.SingleOrDefault(service =>
                        service.ServiceType == typeof(DbContextOptions<AppDbContext>)
                    );
                    services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    services.AddMvc(option => option.Filters.Add(new FakeUserFilter()));
                    services.Remove(dbContextOptions);
                    services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("TestDb"));
                });
            });
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task GetCreate_Always_ReturnsCreateView()
        {
            // Arrange & Act
            var defaultPage = await _client.GetAsync("/Recipes/Create");

            // Assert
            Assert.Equal(HttpStatusCode.OK,defaultPage.StatusCode);

        }
    }
}
