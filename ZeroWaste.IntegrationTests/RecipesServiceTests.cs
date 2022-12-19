using Microsoft.AspNetCore.Mvc.Testing;
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
    }
}
