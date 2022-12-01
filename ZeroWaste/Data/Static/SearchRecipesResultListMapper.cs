using ZeroWaste.DapperModels;
using ZeroWaste.Data.Structs;

namespace ZeroWaste.Data.Static;

public static class SearchRecipesResultListMapper
{
    public static List<RecipeResult> MapToRecipeResult(this List<SearchRecipesResults> searchByIngredientsResultsList)
    {
        List<RecipeResult> recipeResultsList = new List<RecipeResult>();        
        List<int> recipesIdList = searchByIngredientsResultsList
            .Select(x => x.RecipeId)
            .Distinct()
            .ToList();
        foreach (var recipe in recipesIdList)
        {
            RecipeResult recipeResult = GetSingleRecipe(recipe, searchByIngredientsResultsList);
            recipeResultsList.Add(recipeResult);
        }
        return recipeResultsList;
    }    
    private static RecipeResult GetSingleRecipe(int id, List<SearchRecipesResults> searchByIngredientsResults)
    {
        var item = searchByIngredientsResults
            .Where(x => x.RecipeId == id)
            .Select(x => new
            {
                x.RecipeId,
                x.Title,
                x.EstimatedTime,
                x.DifficultyLevel,
                x.CategoryId,
                x.Stars
            }).FirstOrDefault();
        return new RecipeResult()
        {
            Id = item.RecipeId,
            Title = item.Title,
            EstimatedTime = item.EstimatedTime,
            DifficultyLevel = item.DifficultyLevel,
            CategoryId = item.CategoryId,
            Stars = item.Stars,
            Ingredients = GetRecipeIngredient(item.RecipeId, searchByIngredientsResults)
        };
    }    
    private static List<string?> GetRecipeIngredient(int id, List<SearchRecipesResults> searchByIngredientsResultsList)
    {
        var items = searchByIngredientsResultsList
            .Where(x => x.RecipeId == id)
            .Select(x => x.IngredientName)
            .ToList();
        return items;
    }
}