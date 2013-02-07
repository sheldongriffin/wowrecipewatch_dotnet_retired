ALTER TABLE [dbo].[CharacterTradeskills]
	ADD CONSTRAINT [FK_CharacterTradeskills_Tradeskills]
	FOREIGN KEY ([TradeskillId])
	REFERENCES [Tradeskills] ([TradeskillId])
