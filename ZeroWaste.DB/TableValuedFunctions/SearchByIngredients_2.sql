/****** Object:  UserDefinedFunction [dbo].[SearchByIngredients_2]    Script Date: 01.12.2022 19:23:57 ******/
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
CREATE OR ALTER   FUNCTION [dbo].[SearchByIngredients_2]
(
	@IdsVarchar VARCHAR(MAX), @UserId VARCHAR(450)
)
RETURNS 
@Result_Table TABLE 
(
	RecipeId INT,
	Title VARCHAR(1000),
	EstimatedTime INT,
	DifficultyLevel INT,
	Stars FLOAT,
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
	SELECT	T.RecipeId AS [RecipeId],
			Title AS [Title],
			T.EstimatedTime AS [EstimatedTime],
			T.DifficultyLevel AS [DifficultyLevel],
			ISNULL((SELECT 
						AVG(Cast(Stars as Float))
				   FROM dbo.RecipeReviews
				   WHERE RecipeId = T.RecipeId
				   GROUP BY RecipeId), 0) AS [Stars],
			T.CategoryId AS [CategoryId],
			Ingredients.Name AS [IngredientName],
			UnitOfMeasures.Shortcut AS [UnitShortcut],
			RI.Quantity AS [Quantity],
			CAST(ISNULL(IdTable.ID,0) AS BIT) AS [IsMatch],
			MatchIngredientsCount AS [MissingIngredientsCount]
	FROM	(SELECT	(SELECT	COUNT(1)
					FROM	dbo.RecipeIngredients AS RI
					WHERE	RI.RecipeId = Recipes.Id) AS IngredientsCount,
					(SELECT	COUNT(1)
					FROM	dbo.RecipeIngredients AS RI
					JOIN	@IdTable as IdTable
					ON		RI.IngredientId = IdTable.ID
					WHERE	RI.RecipeId = Recipes.Id	) AS MatchIngredientsCount,
					Recipes.Id AS RecipeId,
					Title,
					Description,
					EstimatedTime,
					DifficultyLevel,
					CategoryId
			FROM	dbo.Recipes
			WHERE	StatusId = 1
			AND		NOT EXISTS (SELECT	RecIng.RecipeId
								FROM	DBO.HarmfulIngredients AS HI
								JOIN	DBO.RecipeIngredients AS RecIng 
								ON		RecIng.IngredientId = HI.IngredientId
								WHERE	UserId = @UserId
								AND		RecIng.RecipeId = Recipes.Id)
			)T
	INNER JOIN	dbo.RecipeIngredients AS RI
	ON			RI.RecipeId = T.RecipeId
	LEFT JOIN	@IdTable as IdTable
	ON			RI.IngredientId = IdTable.ID
	INNER JOIN	dbo.Ingredients
	ON			Ingredients.Id = RI.IngredientId
	INNER JOIN	dbo.UnitOfMeasures
	ON			Ingredients.UnitOfMeasureId = UnitOfMeasureS.Id
	WHERE		MatchIngredientsCount > 0 

    RETURN
END
