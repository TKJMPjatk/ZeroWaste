SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tomasz Krasieńko
-- Create date: 20221117
-- Description:	Returns ingredient with column which show ingredient is in shoppping list
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[GetIngredientsToAddToShoppingList]
(
	  @ShoppingListId INT
	, @SearchQuery VARCHAR(400)
)
RETURNS 
@OUTPUT TABLE 
(
	  Id INT
	, [Name] NVARCHAR(400)
	, [Description] NVARCHAR(MAX)
	, [IngredientTypeId] INT
	, [UnitOfMeasureId] INT
	, [IsAdded] BIT
)
AS
BEGIN
	IF(LEN(@SearchQuery) = 0)
	BEGIN
		INSERT INTO @OUTPUT
		SELECT 
			  Ingredients.Id AS [Id]
			, Ingredients.Name AS [Name]
			, Ingredients.Description AS [Description]
			, Ingredients.IngredientTypeId AS [IngredientTypeId]
			, Ingredients.UnitOfMeasureId AS [UnitOfMeasureId]
			, CAST(CASE 
					WHEN ShoppingListIngredients.Id IS NULL THEN 0 
					WHEN ShoppingListIngredients.Id IS NOT NULL THEN 1 
				   END AS BIT) AS [IsAdded]
		FROM dbo.Ingredients
		LEFT JOIN (SELECT 
						Id
					  , IngredientId 
				  FROM dbo.ShoppingListIngredients
				  WHERE 1=1
				  AND ShoppingListId = @ShoppingListId)ShoppingListIngredients ON Ingredients.Id = ShoppingListIngredients.IngredientId
	END
	ELSE
	BEGIN
		DECLARE @SearchQueryPlus VARCHAR(400) = '"*'+@SearchQuery+'*"'
		INSERT INTO @OUTPUT
		SELECT 
			  Ingredients.Id AS [Id]
			, Ingredients.Name AS [Name]
			, Ingredients.Description AS [Description]
			, Ingredients.IngredientTypeId AS [IngredientTypeId]
			, Ingredients.UnitOfMeasureId AS [UnitOfMeasureId]
			, CAST(CASE 
					WHEN ShoppingListIngredients.Id IS NULL THEN 0 
					WHEN ShoppingListIngredients.Id IS NOT NULL THEN 1 
				   END AS BIT) AS [IsAdded]
		FROM dbo.Ingredients
		LEFT JOIN (SELECT 
						Id
					  , IngredientId 
				  FROM dbo.ShoppingListIngredients
				  WHERE 1=1
				  AND ShoppingListId = @ShoppingListId)ShoppingListIngredients ON Ingredients.Id = ShoppingListIngredients.IngredientId
		WHERE 1=1
		  AND CONTAINS(Ingredients.[Name], @SearchQueryPlus)
	END
	RETURN 
END
