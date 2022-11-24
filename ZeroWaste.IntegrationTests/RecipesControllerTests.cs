using System.Net;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZeroWaste.Data;
using ZeroWaste.Data.ViewModels.ExistingRecipe;
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
            var defaultPage = await _client.GetAsync("/Recipes/Create");
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
        }
        [Fact]
        public async Task GetDetails_WhenRecipeIsNotFound_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/Recipes/Details/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task GetDetails_WhenRecipeFound_ReturnsDetailsView()
        {
            var response = await _client.GetAsync("/Recipes/Details/1");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task GetDetails_WhenRecipeUnconfirmed_ReturnsUnauthorizedView()
        {
            var response = await _client.GetAsync("/Recipes/Details/2");
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
        [Fact]
        public async Task GetEdit_WhenRecipeNotExists_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/Recipes/Edit/999");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        [Fact]
        public async Task GetEdit_WhenUserIsAuthor_ReturnsUnauthorized()
        {
            int recipeId = AddBlankRecipeWithFakeAuthor();
            var response = await _client.GetAsync($"/Recipes/Edit/{recipeId}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task PostEdit_ForInvalidModel_ReturnsEditView()
        {
            EditRecipeVM editRecipe = new();
            List<IFormFile> formFiles = new();
            object[] content = { editRecipe, formFiles };
            var httpContent = content.ToJsonHttpContent();
            var response = await _client.PostAsync("/Recipes/Edit", httpContent);
            var absolutPath = response.RequestMessage.RequestUri.AbsolutePath;
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("/Recipes/Edit", response.RequestMessage.RequestUri.AbsolutePath);
        }
        [Fact]
        public async Task PostEdit_ForValidModel_RedirectToRecipeIngredients()
        {
            // Arrange
            int recipeId = AddBlankRecipeWithFakeAuthor();
            var defaultPage = await _client.GetAsync($"/Recipes/Edit/{recipeId}");
            var content = await HtmlHelpers.GetDocumentAsync(defaultPage);

            // Act
            var response = await _client.SendAsync(
            (IHtmlFormElement)content.QuerySelector("form[id='RecipeEdit']"),
            (IHtmlButtonElement)content.QuerySelector("button[id='RecipeEditSubmit']"));

            // Assert
            Assert.Equal(HttpStatusCode.OK, defaultPage.StatusCode);
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
            Assert.Equal("/", response.Headers.Location.OriginalString);

            //EditRecipeVM recipe = ValidEditObject();

            //var postRequest = new HttpRequestMessage(HttpMethod.Post, "/Recipes/Edit");
            ////var formModel = new Dictionary<string, string>
            ////{
            ////    { "Name", "New Employee" },
            ////    { "Age", "25" }
            ////};
            ////var val = new StringContent(JsonConvert.SerializeObject(recipe));
            //var httpContent = recipe.ToJsonHttpContent();
            //postRequest.Content = httpContent;//new FormUrlEncodedContent(formModel);

            //var response = await _client.SendAsync(postRequest);

            //response.EnsureSuccessStatusCode();

            //var responseString = await response.Content.ReadAsStringAsync();

            //Assert.Contains("Account number is required", responseString);



            //var httpContent = recipe.ToJsonHttpContent();
            //var response = await _client.PostAsync("/Recipes/Edit/1", httpContent);
            //var absolutPath = response.RequestMessage.RequestUri.AbsolutePath;
            //Assert.Equal("/ShoppingListIngredients/IngredientsToAdd/1", absolutPath);
            //Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        private int AddBlankRecipeWithFakeAuthor()
        {
            var recipe = new Recipe()
            {
                AuthorId = "1",
                Description = "Lorem ipsum...",
                Title = "Ala ma kota",
                StatusId = 1
            };
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<AppDbContext>();
            _dbContext.Recipes.Add(recipe);
            _dbContext.SaveChanges();
            return recipe.Id;
        }

        private EditRecipeVM ValidEditObject()
        {
            EditRecipeVM editRecipe = new()
            {
                Id = 1,
                Title = "Po zmianie",
                Description = "dupa",
                EstimatedTime = 1,
                DifficultyLevel = 5,
                CategoryId = 1,
                PhotosToDelete = "1|2|3",
                NewPhotosNamesToSkip = "A.jpg|b.bmp|c",
                filesUpload = GenerateFormFile()
            };
            return editRecipe;
        }

        private IEnumerable<IFormFile> GenerateFormFile()
        {
            var ret = new List<IFormFile>();
            try
            {
                using var stream = File.OpenRead("..\\..\\..\\Image\\KatieSecurity.jpg");
                var file = new FormFile(stream, 0, stream.Length, Path.GetFileName(stream.Name), Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpg"
                };
                ret.Add(file);
            }
            catch (Exception) { }
            return ret;
        }
    }
}
