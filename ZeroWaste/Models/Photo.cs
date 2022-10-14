using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class Photo
    {
        public int Id { get; set; }
        public byte[] BinaryPhoto { get; set; }
        public DateTime CreatedAt { get; set; }

        public int? RecipeReviewId { get; set; }
        [ForeignKey(nameof(RecipeReviewId))]
        public RecipeReview? RecipeReview { get; set; }

        public int? RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public Recipe? Recipe { get; set; }
    }
}
