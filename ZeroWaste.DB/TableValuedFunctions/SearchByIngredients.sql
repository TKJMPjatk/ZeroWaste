-- ==================================================================
-- Create Multi-Statement Function template for Azure SQL Database
-- ==================================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jakub Michalak
-- Author:		Tomasz Krasienko
-- Create date: 2022.11.07
-- Description:	function that returns a list of recipes based on the given ingredient id
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[SearchByIngredients]
(
	@IdsVarchar VARCHAR(MAX)
)
RETURNS 
@Result_Table TABLE 
(
	RecipeId INT,
	Title VARCHAR(1000),
	EstimatedTime INT,
	DifficultyLevel INT,
	CategoryId INT,
	IngredientName VARCHAR(400),
	UnitOfMeasureShortcut VARCHAR(100),
	IngredientQuantity FLOAT,
	Match BIT,
	MissingIngredientsCount INT
)
AS
BEGIN
	DECLARE @IdTable TABLE
	(ID INT)


	INSERT INTO	@IdTable
	SELECT		*
	FROM		string_split(@IdsVarchar,',')

	INSERT INTO @Result_Table
	SELECT	T.RecipeId,
			Title,
			T.EstimatedTime,
			T.DifficultyLevel,
			T.CategoryId,
			Ingredients.Name,
			UnitOfMeasures.Shortcut,
			RecipeIngredients.Quantity,
			CAST(ISNULL(IdTable.ID,0) AS BIT) AS Match,
			IngredientsCount-MatchIngredientsCount AS MissingIngredientsCount
	FROM(
		SELECT	(SELECT	COUNT(1)
				FROM	dbo.RecipeIngredients AS RI
				WHERE	RI.RecipeId = Recipes.Id) AS IngredientsCount,
				(SELECT	COUNT(1)
				FROM	dbo.RecipeIngredients AS RI
				JOIN	@IdTable as IdTable
					ON	RI.IngredientId = IdTable.ID	
				WHERE	RI.RecipeId = Recipes.Id) AS MatchIngredientsCount,
				Recipes.Id AS RecipeId,
				Title,
				Description,
				EstimatedTime,
				DifficultyLevel,
				CategoryId
		FROM	dbo.Recipes
		)T
	INNER JOIN	dbo.RecipeIngredients
			ON	RecipeIngredients.RecipeId = T.RecipeId
	LEFT JOIN	@IdTable as IdTable
			ON	RecipeIngredients.IngredientId = IdTable.ID
	INNER JOIN	dbo.Ingredients
			ON	Ingredients.Id = RecipeIngredients.IngredientId
	INNER JOIN	dbo.UnitOfMeasures
			ON	Ingredients.UnitOfMeasureId = UnitOfMeasureS.Id
	WHERE	MatchIngredientsCount > 0

	RETURN
END
GO
