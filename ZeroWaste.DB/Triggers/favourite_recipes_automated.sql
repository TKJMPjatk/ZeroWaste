CREATE TRIGGER dbo.favourite_recipes_automated
    ON dbo.FavouriteRecipes
    FOR INSERT, UPDATE
                    AS
DECLARE @recipe_id INT, @user_id VARCHAR(450)
SELECT
        @recipe_id = RecipeId
     , @user_id = UserId
FROM inserted
DELETE FROM dbo.HatedRecipes
WHERE 1=1
  AND RecipeId = @recipe_id
  AND UserId = @user_id