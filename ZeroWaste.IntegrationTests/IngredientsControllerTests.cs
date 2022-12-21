using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeroWaste.Data;
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels.NewIngredient;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests
{
    [Collection("#1")]
    public class IngredientsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly string _connectionString;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly AppDbContext _dbContext;
        private readonly IIngredientsService _ingredientsService;

        public IngredientsControllerTests(WebApplicationFactory<Program> factory)
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
                        var IDbConnection = services.Where(s => s.ServiceType == typeof(IDbConnectionFactory)).ToList();
                        foreach (var item in IDbConnection)
                        {
                            services.Remove(item);
                        }
                        services.AddScoped<IDbConnectionFactory, TestDbConnectionFactory>();
                    });
                });
            var serviceScope = _factory.Services.CreateScope();
            _ingredientsService = serviceScope.ServiceProvider.GetService<IIngredientsService>();
            _dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
        }

        [Fact]
        public async Task AddNewIngredient_ForValidModel_ShouldReturnNotNull()
        {
            // Arrange
            NewIngredientVM newIngredientVM = new()
            {
                Name = "test123",
                Description = "deeeeeeeeeee",
                IngredientTypeId = 1,
                UnitOfMeasureId = 1
            };

            // Act
            await _ingredientsService.AddNewAsync(newIngredientVM);
            var ingredientByName = await _dbContext.Ingredients.Where(c => c.Name == "test123").FirstOrDefaultAsync();

            // Assert
            Assert.NotNull(ingredientByName);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task AddNewIngredient_ForInvalidModel_ShouldReturnNotNull()
        {
            // Arrange
            NewIngredientVM newIngredientVM = new();

            // Act & Arrange
            await Assert.ThrowsAnyAsync<DbUpdateException>(async () =>await _ingredientsService.AddNewAsync(newIngredientVM));
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task AddNewIngredient_ForValidModel_ShouldReturnsId()
        {
            // Arrange
            int ingredientId;
            string guid = Guid.NewGuid().ToString();

            // Act
            ingredientId = await AddIngredient(guid);
            var ingredientByName = await _dbContext.Ingredients.Where(c => c.Name == guid).FirstOrDefaultAsync();

            // Assert
            Assert.Equal(ingredientId, ingredientByName.Id);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task DeleteAsync_ForExistingIngredient_ShouldReturnsNull()
        {
            // Arrange
            int ingredientId;
            string guid = Guid.NewGuid().ToString();

            // Act
            ingredientId = await AddIngredient(guid);
            await _ingredientsService.DeleteAsync(ingredientId);
            var ingredientByName = await _dbContext.Ingredients.Where(c => c.Name == guid).FirstOrDefaultAsync();

            // Assert
            Assert.Null(ingredientByName);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task GetNewIngredientDropdownsWM_Always_TypesShouldNotBeEmpty()
        {
            NewIngredientDropdownsWM dropdowns;

            dropdowns = await _ingredientsService.GetNewIngredientDropdownsWM();
            Assert.NotEmpty(dropdowns.IngredientTypes);
        }

        [Fact]
        public async Task GetNewIngredientDropdownsWM_Always_UnitsShouldNotBeEmpty()
        {
            NewIngredientDropdownsWM dropdowns;

            dropdowns = await _ingredientsService.GetNewIngredientDropdownsWM();
            Assert.NotEmpty(dropdowns.UnitOfMeasures);
        }

        [Fact]
        public async Task UpdateAsync_Always_UpdateName()
        {
            int ingredientId;
            string guid = Guid.NewGuid().ToString();

            ingredientId = await AddIngredient(guid);
            NewIngredientVM newIngredientVM = new()
            {
                Name = guid,
                Description = "Raz w szynelu się zderzywszy wyszły równym rzędem trzy wszy.",
                IngredientTypeId = 1,
                UnitOfMeasureId = 1,
                Id = ingredientId
            };
            await _ingredientsService.UpdateAsync(newIngredientVM);

            Assert.Equal("Raz w szynelu się zderzywszy wyszły równym rzędem trzy wszy.", newIngredientVM.Description);
            await CleanDb.Clean(_connectionString);
        }

        private async Task<int> AddIngredient(string guid)
        {
            
            Ingredient ingredient = new()
            {
                Name = guid,
                Description = "Żubr żuł żuchwą żurawinę, mając przy tym kwaśną minę.",
                IngredientTypeId = 1,
                UnitOfMeasureId = 1
            };
            await _dbContext.AddAsync(ingredient);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(ingredient).State = EntityState.Detached;
            return ingredient.Id;
        }
    }
}
