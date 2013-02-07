ALTER TABLE [dbo].[Characters]
	ADD CONSTRAINT [FK_Characters_Races]
	FOREIGN KEY ([RaceId])
	REFERENCES [Races] ([RaceId])
