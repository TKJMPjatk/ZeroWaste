namespace ZeroWaste.DapperModels;

public class SearchByIngredientsResults
{
    public int RecipeId { get; set; }
    public string? Title { get; set; }
    public int EstimatedTime { get; set; }
    public int DifficultyLevel { get; set; }
    public double Stars { get; set; }
    public int CategoryId { get; set; }
    public string? IngredientName { get; set; }
    public string? UnitOfMeasureShortcut { get; set; }
    public double IngredientQuantity { get; set; }
    public bool Match { get; set; }
    public int MissingIngredientCount { get; set; }
}