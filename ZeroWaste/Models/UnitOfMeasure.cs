namespace ZeroWaste.Models
{
    public class UnitOfMeasure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Shortcut { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
