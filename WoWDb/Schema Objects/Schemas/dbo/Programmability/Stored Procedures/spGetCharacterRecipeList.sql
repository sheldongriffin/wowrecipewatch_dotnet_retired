CREATE PROCEDURE [dbo].[spGetCharacterRecipeList]
AS
	SELECT c.Name AS CharacterName, t.Name AS TradeskillName, r.Name AS RecipeName, r.*, r.Source, i.Quality, i.Icon, CASE WHEN cr.RecipeId IS NULL THEN 0 ELSE 1 END AS HasRecipe FROM Recipes r
	INNER JOIN (
		SELECT DISTINCT c.CharacterId, TradeskillId
		FROM Characters c INNER JOIN CharacterRecipes cr ON c.CharacterId = cr.CharacterId
		INNER JOIN Recipes r ON cr.RecipeId = r.RecipeId
		GROUP BY c.CharacterId, TradeskillId
	) a ON r.TradeskillId = a.TradeskillId
	INNER JOIN Characters c ON a.CharacterId = c.CharacterId
	LEFT JOIN Tradeskills t ON r.TradeskillId = t.TradeskillId
	LEFT JOIN CharacterRecipes cr ON c.CharacterId = cr.CharacterId AND cr.RecipeId = r.RecipeId
	LEFT JOIN Items i ON r.ItemId = i.ItemId
	WHERE cr.RecipeId IS NULL
	ORDER BY a.CharacterId, t.[Primary] DESC, r.TradeskillId, r.RecipeId