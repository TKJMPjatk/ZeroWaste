﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public double Quantity { get; set; }

        public int RecipeId { get; set; }
        [ForeignKey(nameof(RecipeId))]
        public Recipe Recipe { get; set; }


        public int IngredientId { get; set; }
        [ForeignKey(nameof(IngredientId))]
        public Ingredient Ingredient { get; set; }
    }
}
