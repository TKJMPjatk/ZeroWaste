using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ZeroWaste.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa produktu")]
        public string Name { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }

        public int IngredientTypeId { get; set; }
        [ForeignKey(nameof(IngredientTypeId))]
        public IngredientType IngredientType { get; set; }

        public int UnitOfMeasureId { get; set; }
        [ForeignKey(nameof(UnitOfMeasureId))]
        public UnitOfMeasure UnitOfMeasure { get; set; }

        public List<RecipeIngredient> RecipesIngredients { get; set; }

        public List<ShoppingListIngredient> ShoppingListIngredients { get; set; }
        public List<HarmfulIngredient> HarmfulIngredients { get; set; }
    }
}
