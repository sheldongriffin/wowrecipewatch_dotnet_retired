CREATE PROCEDURE [dbo].[spGetCharacterRecipes] @CharacterName nvarchar(50), @TradeskillId int, @HasRecipe bit
AS
BEGIN

	DECLARE @RealmId int
	DECLARE @CharacterId int
	DECLARE @MinBuyoutTable TABLE (RecipeId int, MinBuyout bigint)
	
	INSERT INTO @MinBuyoutTable (RecipeId, MinBuyout)
	SELECT RecipeId, MIN(buyout) from Items i 
	INNER JOIN Auctions a on i.ItemId = a.ItemId
	WHERE a.Faction = 'Alliance'
	GROUP BY recipeid
	
	SELECT @RealmId = RealmId, @CharacterId = CharacterId FROM Characters WHERE Name = @CharacterName
	
	IF @HasRecipe = 1
		SELECT
			r.Name as SkillName,
			i.ItemId AS RecipeItemId,
			r.ItemId AS CraftedItemId,
			j.Icon,
			r.RecipeId AS SpellId,
			j.Quality,
			r.Source,
			i.ServerwideAverage,
			@HasRecipe AS HasRecipe,
			--CASE WHEN cr.CharacterId IS NULL THEN 0 ELSE 1 END AS HasRecipe,
			mb.MinBuyout,
			0 AS MaterialPrice,
			0 AS CraftedPrice,
			0 AS PotentialProfit
		FROM Recipes r
		LEFT JOIN items i on r.RecipeId = i.RecipeId
		LEFT JOIN Items j ON r.ItemId = j.ItemId
		LEFT JOIN @MinBuyoutTable mb ON r.RecipeId = mb.RecipeId
		--LEFT JOIN MaterialPrices mp ON (r.SpellId = mp.RecipeId AND mp.RealmId = @RealmId AND mp.Faction = 'Alliance')
		--LEFT JOIN MarketPrices ap ON (r.CraftedItemId = ap.ItemId AND ap.RealmId = @RealmId AND ap.Faction = 'Alliance')
		--LEFT JOIN CharacterRecipes cr ON (r.RecipeId = cr.RecipeId AND cr.CharacterId = @CharacterId)
		WHERE TradeskillId = @TradeskillId 
		AND r.RecipeId IN (SELECT RecipeId FROM CharacterRecipes WHERE CharacterId = @CharacterId)
		--AND (
		--	(cr.CharacterId IS NOT NULL) -- Already has the recipe
		--	OR
		--	( ISNULL(r.Source, '') NOT IN ( 'Removed', 'Really Removed' ) ) -- Doesn't have it, but it hasn't been removed
		--	OR
		--	( ISNULL(r.Source, '') = 'Removed' AND i.BindOnPickup = 0) -- Doesn't have it, it's been removed, but recipe is not bind on pickup
		--)
		ORDER BY CASE WHEN r.Source = 'Vendor' THEN 99736510100 ELSE
		(SELECT ISNULL(MIN(buyout), 99736510100) FROM auctions a WHERE a.ItemId = i.ItemId) END, Source
	ELSE
		SELECT
			r.Name as SkillName,
			i.ItemId AS RecipeItemId,
			r.ItemId AS CraftedItemId,
			j.Icon,
			r.RecipeId AS SpellId,
			j.Quality,
			r.Source,
			i.ServerwideAverage,
			@HasRecipe AS HasRecipe,
			mb.MinBuyout,
			--CASE WHEN cr.CharacterId IS NULL THEN 0 ELSE 1 END AS HasRecipe,
			0 AS MaterialPrice,
			0 AS CraftedPrice,
			0 AS PotentialProfit
		FROM Recipes r
		LEFT JOIN items i on r.RecipeId = i.RecipeId
		LEFT JOIN Items j ON r.ItemId = j.ItemId
		LEFT JOIN @MinBuyoutTable mb ON r.RecipeId = mb.RecipeId
		WHERE TradeskillId = @TradeskillId 
		AND r.RecipeId NOT IN (SELECT RecipeId FROM CharacterRecipes WHERE CharacterId = @CharacterId)		
		ORDER BY CASE WHEN r.Source = 'Vendor' THEN 99736510100 ELSE
		ISNULL(mb.MinBuyout, 99736510100) END, [Source]

	IF @HasRecipe = 1
		SELECT * FROM Auctions a
		INNER JOIN Items i ON a.ItemId = i.ItemId
		INNER JOIN CharacterRecipes cr ON i.RecipeId = cr.RecipeId
		INNER JOIN Recipes r ON i.RecipeId = r.RecipeId
		WHERE cr.CharacterId = @CharacterId AND r.TradeskillId = @TradeskillId AND a.Faction = 'Alliance'
		ORDER BY a.Buyout
	ELSE
		SELECT * FROM Auctions a
		INNER JOIN Items i ON a.ItemId = i.ItemId
		INNER JOIN Recipes r ON i.RecipeId = r.RecipeId
		WHERE r.TradeskillId = @TradeskillId
		AND r.RecipeId NOT IN (SELECT RecipeId FROM CharacterRecipes WHERE CharacterId = @CharacterId) AND a.Faction = 'Alliance'
		ORDER BY a.Buyout
END
GO
