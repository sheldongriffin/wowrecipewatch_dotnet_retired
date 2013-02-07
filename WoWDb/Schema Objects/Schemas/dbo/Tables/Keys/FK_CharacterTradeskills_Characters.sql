ALTER TABLE [dbo].[CharacterTradeskills]
	ADD CONSTRAINT [FK_CharacterTradeskills_Characters]
	FOREIGN KEY ([CharacterId])
	REFERENCES [Characters] ([CharacterId])
