ALTER TABLE [dbo].[Auctions]
    ADD CONSTRAINT [FK_Auctions_Realms] FOREIGN KEY ([RealmId]) REFERENCES [dbo].[Realms] ([RealmId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

