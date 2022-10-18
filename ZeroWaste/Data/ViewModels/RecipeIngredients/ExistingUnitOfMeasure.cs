using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZeroWaste.Data.ViewModels.RecipeIngredients
{
    public class ExistingUnitOfMeasure
    {
        public int Id { get; set; }
        [Display(Name = "Jednostka miary")]
        public string Name { get; set; }
        [Display(Name = "Jednostka miary - skrót")]
        public string Shortcut { get; set; }
    }
}
