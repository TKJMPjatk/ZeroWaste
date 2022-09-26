using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class ShoppingList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }

        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser ApplicationUser { get; set; }

        public List<ShoppingListIngredient> ShoppingListIngredients { get; set; }
    }
}
