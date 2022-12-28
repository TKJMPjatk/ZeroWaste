SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tomasz Krasieńko
-- Create date: 20221117
-- Description:	Returns ingredient with column which show ingredient is in shoppping list
-- =============================================
CREATE FUNCTION [dbo].[SearchByCategory]
(
	  @CategoryId INT
	, @UserId NVARCHAR(450)
)
RETURNS 
@OUTPUT TABLE 
(
	  [RecipeId] INT
	, [Title] NVARCHAR(1000)
	, [EstimatedTime] INT
	, [DifficultyLevel] INT
	, [IngredientName] NVARCHAR(400)
	, [Stars] FLOAT
)
AS
BEGIN
	IF @CategoryId = (SELECT Id FROM DBO.Categories WHERE [Name] LIKE 'Wszystkie')
BEGIN
INSERT INTO @OUTPUT
SELECT
    BaseRecipes.Id AS [RecipeId]
			, BaseRecipes.Title AS [Title]
			, BaseRecipes.EstimatedTime AS [EstimatedTime]
			, BaseRecipes.DifficultyLevel AS [DifficultyLevel]
			, BaseIngredients.Name AS [IngredientName]
			, ISNULL((SELECT 
					AVG(Cast(Stars as Float))
			   FROM dbo.RecipeReviews
			   WHERE RecipeId = BaseRecipes.Id
			   GROUP BY RecipeId), 0) AS [Stars]
FROM dbo.Recipes BaseRecipes
    INNER JOIN dbo.RecipeIngredients BaseRecIng ON BaseRecipes.Id = BaseRecIng.RecipeId
    INNER JOIN dbo.Ingredients BaseIngredients ON BaseRecIng.IngredientId = BaseIngredients.Id
WHERE 1=1
  AND BaseRecipes.Id NOT IN (SELECT
    HatedRecipes.RecipeId
    FROM DBO.HatedRecipes
    WHERE 1=1
  AND HatedRecipes.UserId = @UserId)
  AND BaseRecipes.Id NOT IN (SELECT
    HarmfulRecIng.RecipeId
    FROM dbo.HarmfulIngredients HarmfulIng
    INNER JOIN DBO.RecipeIngredients HarmfulRecIng ON	HarmfulRecIng.IngredientId = HarmfulIng.IngredientId
    WHERE	UserId = @UserId)
  AND BaseRecipes.StatusId = 1
END
ELSE
BEGIN
INSERT INTO @OUTPUT
SELECT
    BaseRecipes.Id AS [RecipeId]
			, BaseRecipes.Title AS [Title]
			, BaseRecipes.EstimatedTime AS [EstimatedTime]
			, BaseRecipes.DifficultyLevel AS [DifficultyLevel]
			, BaseIngredients.Name AS [IngredientName]			
			, ISNULL((SELECT 
					AVG(Cast(Stars as Float))
			   FROM dbo.RecipeReviews
			   WHERE RecipeId = BaseRecipes.Id
			   GROUP BY RecipeId),0) AS [Stars]
FROM dbo.Recipes BaseRecipes
    INNER JOIN dbo.RecipeIngredients BaseRecIng ON BaseRecipes.Id = BaseRecIng.RecipeId
    INNER JOIN dbo.Ingredients BaseIngredients ON BaseRecIng.IngredientId = BaseIngredients.Id
WHERE 1=1
  AND BaseRecipes.Id NOT IN (SELECT
    HatedRecipes.RecipeId
    FROM DBO.HatedRecipes
    WHERE 1=1
  AND HatedRecipes.UserId = @UserId)
  AND BaseRecipes.Id NOT IN (SELECT
    HarmfulRecIng.RecipeId
    FROM dbo.HarmfulIngredients HarmfulIng
    INNER JOIN DBO.RecipeIngredients HarmfulRecIng ON	HarmfulRecIng.IngredientId = HarmfulIng.IngredientId
    WHERE	UserId = @UserId)
  AND BaseRecipes.CategoryId = @CategoryId
  AND BaseRecipes.StatusId = 1
END
	RETURN
END
GO
