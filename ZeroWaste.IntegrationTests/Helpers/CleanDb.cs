
using Dapper;
using Microsoft.Data.SqlClient;

namespace ZeroWaste.IntegrationTests.Helpers;

public class CleanDb
{
     public static void Clean(string connectionString)
    {
        string query = @"BEGIN TRAN

ALTER TABLE [dbo].[RecipeReviews] DROP CONSTRAINT [FK_RecipeReviews_Recipes_RecipeId]
ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_Recipes_RecipeId]
ALTER TABLE [dbo].[FavouriteRecipes] DROP CONSTRAINT [FK_FavouriteRecipes_Recipes_RecipeId]
ALTER TABLE [dbo].[HatedRecipes] DROP CONSTRAINT [FK_HatedRecipes_Recipes_RecipeId]
ALTER TABLE [dbo].[RecipeIngredients] DROP CONSTRAINT [FK_RecipeIngredients_Recipes_RecipeId]
ALTER TABLE [dbo].[Photos] DROP CONSTRAINT [FK_Photos_RecipeReviews_RecipeReviewId]
ALTER TABLE [dbo].[ShoppingListIngredients] DROP CONSTRAINT [FK_ShoppingListIngredients_ShoppingLists_ShoppingListId]
ALTER TABLE [dbo].[ShoppingListIngredients] DROP CONSTRAINT [FK_ShoppingListIngredients_Ingredients_IngredientId]
ALTER TABLE [dbo].[RecipeIngredients] DROP CONSTRAINT [FK_RecipeIngredients_Ingredients_IngredientId]
ALTER TABLE [dbo].[HarmfulIngredients] DROP CONSTRAINT [FK_HarmfulIngredients_Ingredients_IngredientId]

TRUNCATE TABLE dbo.FavouriteRecipes 
TRUNCATE TABLE dbo.HatedRecipes
TRUNCATE TABLE dbo.RecipeIngredients
TRUNCATE TABLE dbo.HarmfulIngredients
TRUNCATE TABLE dbo.Recipes
TRUNCATE TABLE dbo.Photos
TRUNCATE TABLE dbo.RecipeReviews
TRUNCATE TABLE dbo.ShoppingListIngredients
TRUNCATE TABLE dbo.ShoppingLists
TRUNCATE TABLE dbo.Ingredients

ALTER TABLE [dbo].[RecipeReviews]  WITH CHECK ADD  CONSTRAINT [FK_RecipeReviews_Recipes_RecipeId] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipes] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[RecipeReviews] CHECK CONSTRAINT [FK_RecipeReviews_Recipes_RecipeId]
ALTER TABLE [dbo].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_Recipes_RecipeId] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipes] ([Id])
ALTER TABLE [dbo].[Photos] CHECK CONSTRAINT [FK_Photos_Recipes_RecipeId]
ALTER TABLE [dbo].[FavouriteRecipes]  WITH CHECK ADD  CONSTRAINT [FK_FavouriteRecipes_Recipes_RecipeId] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipes] ([Id])
ALTER TABLE [dbo].[FavouriteRecipes] CHECK CONSTRAINT [FK_FavouriteRecipes_Recipes_RecipeId]
ALTER TABLE [dbo].[HatedRecipes]  WITH CHECK ADD  CONSTRAINT [FK_HatedRecipes_Recipes_RecipeId] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipes] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[HatedRecipes] CHECK CONSTRAINT [FK_HatedRecipes_Recipes_RecipeId]
ALTER TABLE [dbo].[RecipeIngredients]  WITH CHECK ADD  CONSTRAINT [FK_RecipeIngredients_Recipes_RecipeId] FOREIGN KEY([RecipeId])
REFERENCES [dbo].[Recipes] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[RecipeIngredients] CHECK CONSTRAINT [FK_RecipeIngredients_Recipes_RecipeId]
ALTER TABLE [dbo].[Photos]  WITH CHECK ADD  CONSTRAINT [FK_Photos_RecipeReviews_RecipeReviewId] FOREIGN KEY([RecipeReviewId])
REFERENCES [dbo].[RecipeReviews] ([Id])
ALTER TABLE [dbo].[Photos] CHECK CONSTRAINT [FK_Photos_RecipeReviews_RecipeReviewId]
ALTER TABLE [dbo].[ShoppingListIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingListIngredients_ShoppingLists_ShoppingListId] FOREIGN KEY([ShoppingListId])
REFERENCES [dbo].[ShoppingLists] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[ShoppingListIngredients] CHECK CONSTRAINT [FK_ShoppingListIngredients_ShoppingLists_ShoppingListId]
ALTER TABLE [dbo].[ShoppingListIngredients]  WITH CHECK ADD  CONSTRAINT [FK_ShoppingListIngredients_Ingredients_IngredientId] FOREIGN KEY([IngredientId])
REFERENCES [dbo].[Ingredients] ([Id])
ALTER TABLE [dbo].[ShoppingListIngredients] CHECK CONSTRAINT [FK_ShoppingListIngredients_Ingredients_IngredientId]
ALTER TABLE [dbo].[RecipeIngredients]  WITH CHECK ADD  CONSTRAINT [FK_RecipeIngredients_Ingredients_IngredientId] FOREIGN KEY([IngredientId])
REFERENCES [dbo].[Ingredients] ([Id])
ALTER TABLE [dbo].[RecipeIngredients] CHECK CONSTRAINT [FK_RecipeIngredients_Ingredients_IngredientId]
ALTER TABLE [dbo].[HarmfulIngredients]  WITH CHECK ADD  CONSTRAINT [FK_HarmfulIngredients_Ingredients_IngredientId] FOREIGN KEY([IngredientId])
REFERENCES [dbo].[Ingredients] ([Id])
ON DELETE CASCADE
ALTER TABLE [dbo].[HarmfulIngredients] CHECK CONSTRAINT [FK_HarmfulIngredients_Ingredients_IngredientId]

COMMIT TRAN";
        using SqlConnection connection = new(connectionString);
        SqlCommand command = new(query, connection);
        command.Connection.Open();
        command.ExecuteNonQuery();
    }
}