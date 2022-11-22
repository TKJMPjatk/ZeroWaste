using ZeroWaste.Data.Structs;
using ZeroWaste.Models;

namespace ZeroWaste.Data.Static;

public static class ListRecipeExtended
{
    public static List<RecipeResult> MapToRecipeResult(this List<Recipe> recipesList)
    {
        List<RecipeResult> recipeResultsList = new();        
        foreach (var item in recipesList)
        {
            RecipeResult recipeResult = new()
            {
                Id = item.Id,
                Title = item.Title,
                CategoryId = item.CategoryId,
                DifficultyLevel = item.DifficultyLevel,
                EstimatedTime = item.EstimatedTime,
                Ingredients = item.RecipesIngredients.Select(x => x.Ingredient.Name).ToList()
            };
            recipeResultsList.Add(recipeResult);
        }
        return recipeResultsList;
    }
}