ALTER TABLE [dbo].[CharacterRecipes]
    ADD CONSTRAINT [FK_CharacterRecipes_Recipes] FOREIGN KEY ([RecipeId]) REFERENCES [dbo].[Recipes] ([RecipeId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

