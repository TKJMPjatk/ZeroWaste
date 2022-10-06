using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ZeroWaste;
using ZeroWaste.Data;
using ZeroWaste.Data.Helpers;
using ZeroWaste.Data.Services;
using ZeroWaste.Data.Services.ShoppingLists;
using ZeroWaste.Models;

var builder = WebApplication.CreateBuilder(args);
var servicesConfiguration = ServicesConfiguration.GetInstance();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString")));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
builder.Services.AddScoped<IUrlQueryHelper, UrlQueryHelper>();
builder.Services.AddScoped<IShoppingListIngredientsHelper, ShoppingListIngredientsHelper>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IShoppingListsService, ShoppingListsService>();
builder.Services.AddScoped<IIngredientsService, IngredientsService>();
builder.Services.AddScoped<IIngredientMapperHelper, IngredientMapperHelper>();
=======

=======
>>>>>>> a0701c7 (Added View and methods to add products to shopping list)
servicesConfiguration.Configure(builder.Services);
>>>>>>> fe03965 (Added service for ingredients in shopping list and added configuration class to DI)
=======
servicesConfiguration.Configure(builder.Services);
>>>>>>> a0701c7b131637b7c8c4cc32b592cc392d6c1808
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


AppDbInitializer.SeedRolesAndUsersAsync(app).Wait();
AppDbInitializer.Seed(app);

app.Run();

