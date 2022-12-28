SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:      Tomasz Krasieńko
-- Create Date: 20221115
-- Description: Procedure change selection of shopping list ingredient by id
-- =============================================
CREATE PROCEDURE [dbo].[ChangeShoppingListIngredientSelection]
(
   @ShoppingListIngredientId INT
)
AS
BEGIN
	DECLARE @IsShoppingListIngredientSelected BIT = (SELECT 
														Selected 
													FROM DBO.ShoppingListIngredients 
													WHERE 1=1 
													  AND Id = @ShoppingListIngredientId)
UPDATE dbo.ShoppingListIngredients
SET Selected = (CASE
                    WHEN @IsShoppingListIngredientSelected = 1 THEN 0
                    WHEN @IsShoppingListIngredientSelected = 0 THEN 1
    END)
WHERE Id = @ShoppingListIngredientId

SELECT
    ShoppingListId
FROM dbo.ShoppingListIngredients
WHERE 1=1
  AND Id = @ShoppingListIngredientId
END
GO
