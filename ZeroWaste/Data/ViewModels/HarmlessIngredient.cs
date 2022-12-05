namespace ZeroWaste.Data.ViewModels
{
    public class HarmlessIngredient
    {
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string IngredientDescription { get; set; }
        public int IngredientTypeId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public string UnitOfMeasureName { get; set; }
        public string UnitOfMeasureShortcut { get; set; }
    }
}
