ALTER TABLE [dbo].[Items]
	ADD CONSTRAINT [FK_Items_Recipes]
	FOREIGN KEY ([RecipeId])
	REFERENCES [Recipes] ([RecipeId])
