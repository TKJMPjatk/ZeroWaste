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
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests
{
    public class RecipesControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly WebApplicationFactory<Program> _factory;

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
                builder.UseSetting("SeedDatabase", "true");
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
        [Fact]
        public async Task GetDetails_RecipeIsNotFound_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/Recipes/Details/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task GetDetails_RecipeFound_ReturnsDetailsView()
        {
            //SeedConfirmedRecipe();
            var response = await _client.GetAsync("/Recipes/Details/1");
            var absolutPath = response.RequestMessage.RequestUri.AbsolutePath;
            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        [Fact]
        public async Task GetDetails_RecipeUnconfirmed_ReturnsUnauthorizedView()
        {
            var response = await _client.GetAsync("/Recipes/Details/3");
            var absolutPath = response.RequestMessage.RequestUri.AbsolutePath;
            var statusCode = response.StatusCode;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        }

        private void SeedConfirmedRecipe()
        {
            var recipeConfirmed = new Recipe()
            {
                Id = 1,
                StatusId = 1,
                AuthorId = "1",
                Description = "a kot ma alę",
                Title = "ala ma kota"
            };
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
            _dbContext.Recipes.Add(recipeConfirmed);
            _dbContext.SaveChanges();
        }
    }
}
