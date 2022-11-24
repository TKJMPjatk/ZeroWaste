using System.ComponentModel.DataAnnotations;
using ZeroWaste.Models;

namespace ZeroWaste.Data.ViewModels.ExistingRecipe
{
    public class EditRecipeVM
    {
        public int Id { get; set; }
        [Display(Name = "Tytuł")]
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        public string Title { get; set; }
        [Display(Name = "Opisz kolejne kroki")]
        [Required(ErrorMessage = "Musisz opisać czytelnikom jak mają przygotować danie")]
        public string Description { get; set; }
        [Display(Name = "Przybliżony czas przygotowania w minutach")]
        [Required(ErrorMessage = "Czas przygotowania jest wymagany")]
        public int EstimatedTime { get; set; }
        [Display(Name = "Poziom trudności")]
        [Required(ErrorMessage = "Określ poziom trudności")]
        public int DifficultyLevel { get; set; }
        [Display(Name = "Kategoria przepisu")]
        [Required(ErrorMessage = "Wskaż kategorię")]
        public int CategoryId { get; set; }
        public List<PhotoVM> Photos { get; set; }
        public string PhotosToDelete { get; set; }
        public string NewPhotosNamesToSkip { get; set; }
        public string PhotoAlt { get; set; } = $"/images/Review/alt.png";
        public IEnumerable<IFormFile> filesUpload { get; set; }
    }
}
