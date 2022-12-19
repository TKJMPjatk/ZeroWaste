using Dapper;
using Microsoft.Data.SqlClient;

namespace ZeroWaste.IntegrationTests.Helpers;

public class CleanDb
{
    private static readonly string _connectionString = "Server=tcp:zero-waste.database.windows.net,1433;Initial Catalog=zeroWaste;Persist Security Info=False;User ID=zerowaste-admin;Password=RUCH200nowe;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
    public static void Clean()
    {
        string query = @"DELETE FROM dbo.FavouriteRecipes 
                        DELETE FROM dbo.HatedRecipes
                        DELETE FROM dbo.RecipeReviews
                        DELETE FROM dbo.RecipeIngredients
                        DELETE FROM dbo.HarmfulIngredients
                        DELETE FROM dbo.Photos
                        DELETE FROM dbo.Recipes
                        DELETE FROM dbo.ShoppingListIngredients
                        DELETE FROM dbo.ShoppingLists
                        DELETE FROM dbo.Ingredients";
        using var connection = new SqlConnection(_connectionString);
        connection.Query(query);
    }
}