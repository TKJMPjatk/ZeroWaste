CREATE TRIGGER dbo.hated_recipes_automated
    ON dbo.HatedRecipes
    FOR INSERT, UPDATE
                    AS
DECLARE @recipe_id INT, @user_id VARCHAR(450)
SELECT
        @recipe_id = RecipeId
     , @user_id = UserId
FROM inserted
DELETE FROM dbo.FavouriteRecipes
WHERE 1=1
  AND RecipeId = @recipe_id
  AND UserId = @user_id
