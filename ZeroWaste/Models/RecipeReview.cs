using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class RecipeReview
    {
        public int Id { get; set; }
        public int Stars { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public string AuthorId { get; set; }
        [ForeignKey(nameof(AuthorId))]
        public ApplicationUser ApplicationUser { get; set; }

        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; set; }
    }
}
