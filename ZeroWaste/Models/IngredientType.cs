﻿namespace ZeroWaste.Models
{
    public class IngredientType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<Ingredient> Ingredients { get; set; }
    }
}
