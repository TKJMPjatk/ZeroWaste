using System.ComponentModel.DataAnnotations;
using ZeroWaste.Data.ViewModels.NewRecepie;

namespace ZeroWaste.Data.ViewModels
{
    public class NewRecipeVM
    {
        public int Id { get; set; }
        [Display(Name="Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        public string Title { get; set; }
        [Display(Name ="Opisz kolejne kroki")]
        [Required(ErrorMessage = "Musisz opisać czytelnikom jak mają przygotować danie")]
        public string Description { get; set; }
        [Display(Name = "Przybliżony czas przygotowania w minutach")]
        [Required(ErrorMessage = "Czas przygotowania jest wymagany")]
        public int EstimatedTime { get; set; }
        [Display(Name = "Poziom trudności")]
        [Required(ErrorMessage = "Określ poziom trudności")]
        public int DifficultyLevel { get; set; }
        [Display(Name = "Wybierz zdjęcia")]
        [Required(ErrorMessage = "Musisz dodać zdjęcie pokazujące potrawę")]
        public List<FileVM> Photos { get; set; }

    }
}
