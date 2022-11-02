using ZeroWaste.Data.Structs;

namespace ZeroWaste.Data.Static;

public static class ListRecipeResultExtended
{
    public static bool IsRecipeResultNullOrEmpty(this List<RecipeResult> recipeResults)
    {
        if (recipeResults is null || recipeResults.Count == 0)
            return true;
        return false;
    }
}