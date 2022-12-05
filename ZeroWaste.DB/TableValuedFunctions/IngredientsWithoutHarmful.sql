-- =========================================================
-- Create Inline Function Template for Azure SQL Database
-- =========================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jakub Michalak
-- Create date: 2022.12.05
-- Description:	function returns all ingredients minus the harmful ones for the parameterized user
-- =============================================
CREATE   FUNCTION [dbo].[IngredientsWithoutHarmful]
(
	@UserId VARCHAR(450)
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT	Ing.Id AS IngredientId,
			Ing.Name AS IngredientName,
			Ing.Description AS IngredientDescription,
			Ing.IngredientTypeId AS IngredientTypeId,
			Ing.UnitOfMeasureId AS UnitOfMeasureId,
			UoM.Name AS UnitOfMeasureName,
			UoM.Shortcut as UnitOfMeasureShortcut
	FROM	dbo.Ingredients AS Ing
	JOIN	dbo.UnitOfMeasures AS UoM
	ON		UoM.Id = Ing.UnitOfMeasureId
	WHERE	NOT EXISTS (SELECT	1
						FROM	dbo.HarmfulIngredients AS HI
						WHERE	HI.UserId = @UserId
						AND		HI.IngredientId = Ing.Id)
)
GO
