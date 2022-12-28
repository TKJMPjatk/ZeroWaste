SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[GetRecipeIdsListForSupriseMe] 
(	
	@UserId VARCHAR(450)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 
		BaseRecipes.Id
	FROM dbo.Recipes BaseRecipes
	WHERE 1=1
	  AND BaseRecipes.Id NOT IN (SELECT 
									RecipeId
								FROM dbo.HatedRecipes
								WHERE UserId = @UserId)
	  AND BaseRecipes.Id NOT IN (SELECT 
									HarmfulRecIng.RecipeId
							   FROM dbo.HarmfulIngredients HarmfulIng
							   INNER JOIN DBO.RecipeIngredients HarmfulRecIng ON HarmfulRecIng.IngredientId = HarmfulIng.IngredientId
							   WHERE	UserId = @UserId)
	  AND BaseRecipes.StatusId = 1
)
GO
