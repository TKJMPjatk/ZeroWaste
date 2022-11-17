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
-- Description:	Returns user safe ingredients
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[IngredientsSafeForUser]
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
	INNER JOIN	DBO.AspNetUsers AS U
	ON			U.Id = HI.UserId
	AND			U.Id = @UserId
	RIGHT JOIN	DBO.Ingredients AS I
	ON			I.Id = HI.IngredientId
	INNER JOIN	DBO.IngredientTypes AS IT
	ON			IT.Id = I.IngredientTypeId
	WHERE		HI.IngredientId IS NULL
)
GO
