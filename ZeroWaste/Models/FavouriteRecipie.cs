using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class FavouriteRecipie
    {
        public int Id { get; set; }

        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
