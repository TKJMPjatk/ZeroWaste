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
using ZeroWaste.Data.DapperConnection;
using ZeroWaste.Data.Handlers.ShoppingListHandlers;
using ZeroWaste.Data.Services.Recipes;
using ZeroWaste.Data.ViewModels;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
using ZeroWaste.Data.ViewModels.NewRecepie;
using ZeroWaste.IntegrationTests.Helpers;
using ZeroWaste.Models;

namespace ZeroWaste.IntegrationTests
{
    [Collection("#1")]
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
                        var IDbConnection = services.Where(s => s.ServiceType == typeof(IDbConnectionFactory)).ToList();
                        foreach (var item in IDbConnection)
                        {
                            services.Remove(item);
                        }
                        services.AddScoped<IDbConnectionFactory, TestDbConnectionFactory>();
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

            // Assert
            Assert.True(result > 0);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task GetById_ForNotExistingData_ShouldReturnsNull()
        {
            // Arrange
            Recipe? recipe;

            // Act
            recipe = await _recipesService.GetByIdAsync(666);

            // Assert
            Assert.Null(recipe);
        }

        [Fact]
        public async Task GetById_ForExistingData_ShouldReturnsCreatedRecipe()
        {
            // Arrange
            Recipe? recipe;

            // Act
            int recipeId = await AddSimpleRecipe();
            recipe = await _recipesService.GetByIdAsync(recipeId);

            // Assert
            Assert.NotNull(recipe);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task GetDropdownsValuesAsync_Always_ShouldReturnsCategories()
        {
            // Arrange
            RecipeDropdownVM recipeDropdownVM;

            // Act
            recipeDropdownVM = await _recipesService.GetDropdownsValuesAsync();

            // Arrange
            Assert.NotEmpty(recipeDropdownVM.Categories);
        }

        [Fact]
        public async Task UpdateAsync_ForExistingRecipe_ShouldUpdateRecipe()
        {
            // Arrange
            int recipeId;
            EditRecipeVM editRecipeVM;
            string userId;

            // Act
            recipeId = await AddSimpleRecipe();
            editRecipeVM = new EditRecipeVM()
            {
                Id = recipeId,
                Title = "def",
                Description = "abc2",
                CategoryId = 1,
                DifficultyLevel = 1,
                EstimatedTime = 1
            };
            userId = _dbContext.Users.First().Id;
            await _recipesService.UpdateAsync(editRecipeVM, userId);

            var updatedRecipe = await _recipesService.GetByIdAsync(recipeId);
            Assert.True(updatedRecipe.Title == "def");
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task AddLiked_ForExistingRecipe_ShouldReturnNotNull()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.AddLiked(recipeId, userId);
            var likedRecipe = await _dbContext.FavouriteRecipes.Where(c => c.UserId == userId && c.RecipeId == recipeId).FirstAsync();

            // Assert
            Assert.NotNull(likedRecipe);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task AddNotLiked_ForExistingRecipe_ShouldReturnNotNull()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.AddNotLiked(recipeId, userId);
            var likedRecipe = await _dbContext.HatedRecipes.Where(c => c.UserId == userId && c.RecipeId == recipeId).FirstAsync();

            // Assert
            Assert.NotNull(likedRecipe);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task GetRecipeIdList_ForOneRecipe_ShouldReturnOneRecipe()
        {
            // Act
            _ = await AddSimpleConfirmedRecipe();
            var recipeIds = await _recipesService.GetRecipeIdList();

            // Assert
            Assert.Single(recipeIds);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task GetRecipeIdList_ForOneHatedRecipe_ShouldReturnZeroRecipes()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.AddNotLiked(recipeId, userId);
            var recipeIds = await _recipesService.GetRecipeIdList(userId);

            // Assert
            Assert.Empty(recipeIds);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task ConfirmRecipe_ForExistingRecipe_ShouldReturnConfirmStatus()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.ConfirmRecipe(recipeId);
            var recipe = await _dbContext.Recipes.Where(c => c.Id == recipeId).FirstAsync();
            Assert.Equal(1, recipe.StatusId);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task RejectRecipe_ForExistingRecipe_ShouldReturnRejectStatus()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.RejectRecipe(recipeId);
            var recipe = await _dbContext.Recipes.Where(c => c.Id == recipeId).FirstAsync();
            Assert.Equal(3, recipe.StatusId);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task UnconfirmRecipe_ForExistingRecipe_ShouldReturnUnconfirmStatus()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.UnconfirmRecipe(recipeId);
            var recipe = await _dbContext.Recipes.Where(c => c.Id == recipeId).FirstAsync();
            Assert.Equal(2, recipe.StatusId);
            await CleanDb.Clean(_connectionString);
        }

        [Fact]
        public async Task UpdateStateAsync_ForExistingRecipe_ShouldReturnUnconfirmStatus()
        {
            // Arrange
            string userId;
            int recipeId;

            // Act
            recipeId = await AddSimpleRecipe();
            userId = _dbContext.Users.First().Id;
            await _recipesService.UpdateStateAsync(recipeId,1);
            var recipe = await _dbContext.Recipes.Where(c => c.Id == recipeId).FirstAsync();
            Assert.Equal(1, recipe.StatusId);
            await CleanDb.Clean(_connectionString);
        }

        private async Task<int> AddSimpleRecipe()
        {
            string userId;
            userId = _dbContext.Users.First().Id;
            var recipe = new Recipe()
            {
                Title = "abc",
                Description = "abc2",
                CategoryId = 1,
                DifficultyLevel = 1,
                EstimatedTime = 1,
                AuthorId = userId,
                StatusId = await _dbContext.Statuses.Where(c => c.Name == "Niepotwierdzony").Select(c => c.Id).FirstOrDefaultAsync()
            };
            await _dbContext.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(recipe).State = EntityState.Detached;
            return recipe.Id;
        }

        private async Task<int> AddSimpleConfirmedRecipe()
        {
            string userId;
            userId = _dbContext.Users.First().Id;
            var recipe = new Recipe()
            {
                Title = "abc",
                Description = "abc2",
                CategoryId = 1,
                DifficultyLevel = 1,
                EstimatedTime = 1,
                AuthorId = userId,
                StatusId = await _dbContext.Statuses.Where(c => c.Name == "Zatwierdzony").Select(c => c.Id).FirstOrDefaultAsync()
            };
            await _dbContext.AddAsync(recipe);
            await _dbContext.SaveChangesAsync();
            _dbContext.Entry(recipe).State = EntityState.Detached;
            return recipe.Id;
        }
    }
}
