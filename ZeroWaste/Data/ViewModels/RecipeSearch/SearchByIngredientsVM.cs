using System.ComponentModel.DataAnnotations;
using ZeroWaste.Data.Structs;

namespace ZeroWaste.Data.ViewModels.RecipeSearch;

public class SearchByIngredientsVm
{
    [Required(ErrorMessage = "Pole jest wymagane")]
    public string Name { get; set; }
    [Required(ErrorMessage = "Pole jest wymagane")]
    [Range(1, Int32.MaxValue, ErrorMessage = "Ilość nie może być mniejsza niż 1")]
    public int Quantity { get; set; }
    public List<IngredientForSearch> SingleIngredientToSearchVm { get; set; } =
        new List<IngredientForSearch>();
}