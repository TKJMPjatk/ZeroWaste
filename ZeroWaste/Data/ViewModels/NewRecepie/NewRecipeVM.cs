using System.ComponentModel.DataAnnotations;

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
        [Range(1, 1000, ErrorMessage = "Wprowadź prawidłowy czas od 1 do 1000")]
        public int? EstimatedTime { get; set; }
        [Display(Name = "Poziom trudności")]
        [Required(ErrorMessage = "Określ poziom trudności")]
        [Range(1, 5, ErrorMessage = "Określ poziom trudności")]
        public int? DifficultyLevel { get; set; }
        [Display(Name = "Kategoria przepisu")]
        [Required(ErrorMessage = "Wskaż kategorię")]
        public int CategoryId { get; set; }
        [Display(Name = "Zdjęcia przepisu")]
        [Required(ErrorMessage = "Zdjęcia są wymagane")]
        public IEnumerable<IFormFile> filesUpload { get; set; }
    }
}
