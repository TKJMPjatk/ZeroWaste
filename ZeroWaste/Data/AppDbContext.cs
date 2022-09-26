using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZeroWaste.Models;

namespace ZeroWaste.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<FavouriteRecipie> FavouriteRecipies { get; set; }
        public DbSet<HarmfulIngredient> HarmfulIngredients { get; set; }
        public DbSet<HatedRecipie> HatedRecipies { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipies { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<RecipePhoto> RecipePhotos { get; set; }
        public DbSet<RecipeReview> RecipeReviews { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListIngredient> ShoppingListIngredients { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
    }
}
