namespace ZeroWaste.Data.Structs;

public class RecipeResult
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int EstimatedTime { get; set; }
    public int DifficultyLevel { get; set; }
    public int CategoryId { get; set; }
    public List<string> Ingredients { get; set; }
    public byte[] Photo { get; set; }
}