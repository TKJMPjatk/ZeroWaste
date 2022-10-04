using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZeroWaste.Models
{
    public class IngredientType
    {
        public int Id { get; set; }
        [Display(Name = "Typ")]
        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
