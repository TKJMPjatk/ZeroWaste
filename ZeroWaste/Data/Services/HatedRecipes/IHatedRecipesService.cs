namespace ZeroWaste.Data.Services.HatedRecipes;

public interface IHatedRecipesService
{
    Task DeleteHatedRecipes(int recipeId, string userId);
}