using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int IngredientTypeId { get; set; }
        [ForeignKey(nameof(IngredientTypeId))]
        public IngredientType IngredientType { get; set; }

        public int UnitOfMeasureId { get; set; }
        [ForeignKey(nameof(UnitOfMeasureId))]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public List<RecipeIngredient> RecipiesIngredients { get; set; }

        public List<ShoppingListIngredient> ShoppingListIngredients { get; set; }
    }
}
