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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Category>()
                .Property(c => c.Name)
                .HasMaxLength(400);
            builder.Entity<Ingredient>()
                .Property(c => c.Name)
                .HasMaxLength(400);
            builder.Entity<IngredientType>()
                .Property(c => c.Name)
                .HasMaxLength(400);
            builder.Entity<UnitOfMeasure>()
                .Property(c => c.Name)
                .HasMaxLength(400);
            builder.Entity<Status>()
                .Property(c => c.Name)
                .HasMaxLength(400);


            base.OnModelCreating(builder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<FavouriteRecipe> FavouriteRecipes { get; set; }
        public DbSet<HarmfulIngredient> HarmfulIngredients { get; set; }
        public DbSet<HatedRecipe> HatedRecipes { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<RecipeReview> RecipeReviews { get; set; }
        public DbSet<ShoppingList> ShoppingLists { get; set; }
        public DbSet<ShoppingListIngredient> ShoppingListIngredients { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<UnitOfMeasure> UnitOfMeasures { get; set; }
        public DbSet<IngredientType> IngredientTypes { get; set; }
    }
}
