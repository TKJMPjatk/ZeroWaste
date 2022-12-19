using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroWaste.Data;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests
{
    public class RecipesServiceTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly string _connectionString;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly AppDbContext _dbContext;
        private readonly IRecipesService _recipesService;

        public RecipesServiceTests(WebApplicationFactory<Program> factory)
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
            _recipesService = serviceScope.ServiceProvider.GetService<IRecipesService>();
            _dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        }

        [Fact]
        public async Task CreateRecipe_ForInvalidModel_ShouldThrowsException()
        {
            // Arrange
            var newRecipe = new NewRecipeVM()
            {
                Title = "abc",
                Description = "abc2"
            };

            // Act & rrange
            await Assert.ThrowsAnyAsync<DbUpdateException>(async () => await _recipesService.AddNewReturnsIdAsync(newRecipe, "xyz"));
        }

        [Fact]
        public async Task CreateRecipe_ForValidModel_ShouldReturnsId()
        {
            // Arrange
            var newRecipe = new NewRecipeVM()
            {
                Title = "abc",
                Description = "abc2",
                CategoryId = 1,
                DifficultyLevel = 1,
                EstimatedTime = 1
            };
            string userId = _dbContext.Users.First().Id;

            // Act
            int result = await _recipesService.AddNewReturnsIdAsync(newRecipe, userId);

            // Arrange
            Assert.True(result > 0);
            CleanDb.Clean(_connectionString);
        }
    }
}
