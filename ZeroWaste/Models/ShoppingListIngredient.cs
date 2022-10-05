using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class ShoppingListIngredient
    {
        public int Id { get; set; }
        public double Quantity { get; set; }
        public bool Selected { get; set; }
        public int ShoppingListId { get; set; }
        [ForeignKey(nameof(ShoppingListId))]
        public ShoppingList ShoppingList { get; set; }

        public int IngredientId { get; set; }
        [ForeignKey(nameof(IngredientId))]
        public Ingredient Ingredient { get; set; }

    }
}
