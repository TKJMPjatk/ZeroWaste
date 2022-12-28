SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Tomasz Krasieńko
-- Create Date: 20221115
-- Description: Procedure change selection of shopping list ingredient by id
-- =============================================
CREATE PROCEDURE [dbo].[AddAutomaticShoppingList]
(
    @Names VARCHAR(MAX),
    @RecipeId INT,
    @UserId VARCHAR(450)
)
AS
BEGIN
    DECLARE @NamesTab TABLE (IngredientName VARCHAR(450))

    INSERT INTO @NamesTab SELECT * FROM STRING_SPLIT(@Names, ',');

DECLARE @ShoppingListTitle VARCHAR(450) = (SELECT TOP 1
                                                Title
                                            FROM dbo.Recipes
                                            WHERE 1=1
                                                AND Id = @RecipeId)
    DECLARE @ShoppingListNote VARCHAR(MAX) = 'Wygenerowane automatycznie na podstawie przepisu ' + @ShoppingListTitle
    DECLARE @AddedIdTab TABLE (AddedId INT) 

    INSERT INTO dbo.ShoppingLists (Title, Note, CreatedAt, UserId)
    OUTPUT inserted.ID INTO @AddedIdTab(AddedId)
    VALUES (@ShoppingListTitle, @ShoppingListNote, GETDATE(), @UserId)

    INSERT INTO dbo.ShoppingListIngredients(Quantity, Selected, ShoppingListId, IngredientId)
SELECT
    RecIng.Quantity
     , 0
     , (SELECT TOP 1
            AddedId
        FROM @AddedIdTab)
     , RecIng.IngredientId
FROM dbo.Recipes Rec
         INNER JOIN dbo.RecipeIngredients RecIng ON Rec.Id = RecIng.RecipeId
WHERE RecIng.IngredientId NOT IN (SELECT
                                      Ing.Id
                                  FROM dbo.Ingredients Ing
                                           INNER JOIN @NamesTab ON Ing.Name = IngredientName)
    (SELECT TOP 1
        AddedId
     FROM @AddedIdTab)
END
GO
