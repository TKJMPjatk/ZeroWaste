-- =========================================================
-- Create Inline Function Template for Azure SQL Database
-- =========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jakub Michalak
-- Create date: 2022.11.16
-- Description:	Returns user harmful ingredients
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[IngredientsHarmfulForUser]
(	
	@UserId NVARCHAR(450)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT		I.Id AS IngredientId,
				I.Name AS IngredientName,
				IT.Name AS IngredientType
	FROM		DBO.HarmfulIngredients AS HI
	INNER JOIN	DBO.Ingredients AS I
	ON			I.Id = HI.IngredientId
	INNER JOIN	DBO.IngredientTypes AS IT
	ON			IT.Id = I.IngredientTypeId
	WHERE		HI.UserId = @UserId
)
GO
