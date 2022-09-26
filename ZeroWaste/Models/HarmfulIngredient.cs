using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class HarmfulIngredient
    {
        public int Id { get; set; }

        public int IngredientId { get; set; }
        [ForeignKey(nameof(IngredientId))]
        public Ingredient Ingredient { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }
    }
}
