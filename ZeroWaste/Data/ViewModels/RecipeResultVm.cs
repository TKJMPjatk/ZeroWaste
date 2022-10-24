using System.ComponentModel.DataAnnotations;

namespace ZeroWaste.Data.ViewModels;

public struct RecipeResultVm
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int EstimatedTime { get; set; }
    public int DifficultyLevel { get; set; }
    public List<string> Ingredients { get; set; }
}