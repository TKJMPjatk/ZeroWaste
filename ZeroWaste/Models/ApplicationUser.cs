﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZeroWaste.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Banned { get; set; }


        public List<Recipe> Recipes { get; set; }
        public List<FavouriteRecipe> FavouriteRecipes { get; set; }
        public List<HatedRecipe> HatedRecipes { get; set; }
        public List<RecipeReview> RecipeReviews { get; set; }
        public List<HarmfulIngredient> HarmfulIngredients { get; set; }
        public List<ShoppingList> ShoppingLists { get; set; }
    }
}