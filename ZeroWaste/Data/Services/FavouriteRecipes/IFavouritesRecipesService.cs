namespace ZeroWaste.Data.Services.FavouriteRecipes;

public interface IFavouritesRecipesService
{
    Task DeleteFromFavourite(int recipeId, string userId);
}