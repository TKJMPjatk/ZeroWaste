using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.RecipeIngredients
{
    public class ExistingIngredient
    {
        public int Id { get; set; }
        [Display(Name = "Nazwa produktu")]
        public string Name { get; set; }
        //[Display(Name = "Opis")]
        //public string Description { get; set; }
        public ExistingUnitOfMeasure UnitOfMeasure { get; set; }
    }
}
