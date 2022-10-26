using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.ExistingRecipe
{
    public class DetailsRecipeVM
    {
        public int Id { get; set; }
        [Display(Name = "Tytuł")]
        public string Title { get; set; }
        [Display(Name = "Kolejne etapy przygotowania")]
        public string Description { get; set; }
        [Display(Name = "Przybliżony czas przygotowania w minutach")]
        public int EstimatedTime { get; set; }
        [Display(Name = "Poziom trudności")]
        public int DifficultyLevel { get; set; }
        [Display(Name = "Kategoria przepisu")]
        public int CategoryId { get; set; }
        [Display(Name = "Zdjęcia przepisu")]
        public List<Photo> Photos { get; set; }
        [Display(Name ="Składniki")]
        public List<RecipeIngredient> RecipesIngredients { get; set; }
        public Category Category { get; set; }
        public List<RecipeReview> RecipeReviews { get; set; }
        public string PhotoAlt { get; set; } = $"/images/Review/alt.png";
        [Display(Name = "Poziom zadowolenia")]
        public int NewReviewStars { get; set; }
        [Display(Name = "Opis")]
        public string NewReviewDescription { get; set; }
        public int NewReviewRecipeId { get; set; }
    }
}
