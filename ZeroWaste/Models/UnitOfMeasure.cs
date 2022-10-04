using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZeroWaste.Models
{
    public class UnitOfMeasure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Display(Name = "Jednostka miary")]
        public string Shortcut { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
