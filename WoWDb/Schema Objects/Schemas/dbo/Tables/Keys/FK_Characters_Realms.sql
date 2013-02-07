ALTER TABLE [dbo].[Characters]
	ADD CONSTRAINT [FK_Characters_Realms]
	FOREIGN KEY ([RealmId])
	REFERENCES [Realms] ([RealmId])
