ALTER TABLE [dbo].[Recipes]
    ADD CONSTRAINT [FK_Recipes_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[Items] ([ItemId]) ON DELETE NO ACTION ON UPDATE NO ACTION;
