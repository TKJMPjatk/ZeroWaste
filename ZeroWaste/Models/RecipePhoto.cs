using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class RecipePhoto
    {
        public int Id { get; set; }
        public string PhotoBinary { get; set; }
        public DateTime CreatedAt { get; set; }

        public int RecipeReviewId { get; set; }
        [ForeignKey(nameof(RecipeReviewId))]
        public RecipeReview RecipeReview { get; set; }
    }
}
