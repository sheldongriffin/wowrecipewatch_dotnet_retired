ALTER TABLE [dbo].[Characters]
	ADD CONSTRAINT [FK_Characters_Classes]
	FOREIGN KEY ([ClassId])
	REFERENCES [Classes] ([ClassId])
