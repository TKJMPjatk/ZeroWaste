CREATE TRIGGER dbo.recipe_ingredients_unique
    ON dbo.RecipeIngredients
    for INSERT, UPDATE
                    AS
DECLARE @recipe_id INT, @ingredient_id INT, @id INT
SELECT @recipe_id = RecipeId, @ingredient_id = IngredientId, @id = Id FROM inserted
DECLARE @rows_number INT
SET @rows_number = (SELECT 
						COUNT(1) 
					FROM dbo.RecipeIngredients 
					WHERE RecipeId = @recipe_id
					  AND IngredientId = @ingredient_id
					  AND Id != @id)
Declare @msg varchar(max) = 'Can not added the same ingredient to shopping list '
IF(@rows_number != 0)
BEGIN
		RAISERROR(@msg, 11,1)
		ROLLBACK
END