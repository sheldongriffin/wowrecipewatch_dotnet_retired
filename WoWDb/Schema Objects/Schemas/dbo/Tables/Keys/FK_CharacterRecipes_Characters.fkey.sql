ALTER TABLE [dbo].[CharacterRecipes]
    ADD CONSTRAINT [FK_CharacterRecipes_Characters] FOREIGN KEY ([CharacterId]) REFERENCES [dbo].[Characters] ([CharacterId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

