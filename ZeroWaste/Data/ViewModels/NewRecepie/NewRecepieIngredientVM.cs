using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace ZeroWaste.Data.ViewModels
{
    public class NewRecepieIngredientVM
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa")]
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        public string Name { get; set; }
        [Display(Name = "Ilość")]
        [Required(ErrorMessage = "Ilość jest wymagana")]
        public double Quantity { get; set; }
        [Display(Name = "Jednostka miary")]
        [Required(ErrorMessage = "Jednostka miary jest wymagana")]
        public string UnitOfMeasure { get; set; }
    }
}
