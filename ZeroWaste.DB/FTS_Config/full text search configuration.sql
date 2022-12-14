CREATE FULLTEXT CATALOG ftCatalog AS DEFAULT;

CREATE FULLTEXT INDEX ON dbo.Ingredients(Name)
KEY INDEX Unique_Ingredients_Name ON ftCatalog; 

ALTER FULLTEXT INDEX ON dbo.Ingredients ENABLE; 
GO 
ALTER FULLTEXT INDEX ON dbo.Ingredients START FULL POPULATION;