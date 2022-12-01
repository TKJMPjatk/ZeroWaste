namespace ZeroWaste.DapperModels;

public class SearchRecipesResults
{
    public int RecipeId { get; set; }
    public string? Title { get; set; }
    public int EstimatedTime { get; set; }
    public int DifficultyLevel { get; set; }
    public int CategoryId { get; set; }
    public string? IngredientName { get; set; }
    public double Stars { get; set; }
}