SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tomasz Krasieńko
-- Create date: 20221117
-- Description:	Returns ingredient with column which show ingredient is in shoppping list
-- =============================================
CREATE FUNCTION [dbo].[SearchFavouriteRecipesByUserId]
(
	  @UserId VARCHAR(450)
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
INSERT INTO @OUTPUT
SELECT
    BaseRecipes.Id AS [RecipeId]
        , BaseRecipes.Title AS [Title]
        , BaseRecipes.EstimatedTime AS [EstimatedTime]
        , BaseRecipes.DifficultyLevel AS [DifficultyLevel]
        , ISNULL(BaseIngredients.Name, '') AS [IngredientName]
        , ISNULL((SELECT 
                    AVG(Cast(Stars as Float))
                    FROM dbo.RecipeReviews
                    WHERE RecipeId = BaseRecipes.Id
                    GROUP BY RecipeId), 0) AS [Stars]
FROM dbo.Recipes BaseRecipes
    INNER JOIN dbo.FavouriteRecipes BaseFav ON BaseRecipes.Id = BaseFav.RecipeId
    LEFT JOIN dbo.RecipeIngredients BaseRecIng ON BaseRecipes.Id = BaseRecIng.RecipeId
    LEFT JOIN dbo.Ingredients BaseIngredients ON BaseRecIng.IngredientId = BaseIngredients.Id
WHERE 1=1
  AND BaseFav.UserId = @UserId
    RETURN
END
GO
