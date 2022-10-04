using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.NewIngredient
{
    public class NewIngredientVM
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa produktu")]
        [Required(ErrorMessage = "Nazwa produktu jest wymagana")]
        public string Name { get; set; }
        [Display(Name = "Krótki opis")]
        [Required(ErrorMessage = "Opis produktu jest wymagany")]
        public string Description { get; set; }
        [Display(Name = "Typ produktu")]
        [Required(ErrorMessage = "Musisz wskazać typ produktu")]
        public int IngredientTypeId { get; set; }
        [Display(Name = "Jednostka miary")]
        [Required(ErrorMessage = "Musisz wskazać jednostkę miary")]
        public int UnitOfMeasureId { get; set; }
    }
}
