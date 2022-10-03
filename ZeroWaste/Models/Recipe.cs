using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EstimatedTime { get; set; }
        public int DifficultyLevel { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category Category { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        public Status Status { get; set; }

        public List<RecipeIngredient> RecipesIngredients { get; set; }
        public List<Photo> Photos { get; set; }
        public List<RecipeReview> RecipeReviews { get; set; }
        public List<HatedRecipe> HatedRecipes { get; set; }
        public List<FavouriteRecipe> FavouriteRecipes { get; set; }
    }
}
