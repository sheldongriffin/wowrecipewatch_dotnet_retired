ALTER TABLE [dbo].[Recipes]
    ADD CONSTRAINT [FK_Recipes_Tradeskills] FOREIGN KEY ([TradeskillId]) REFERENCES [dbo].[Tradeskills] ([TradeskillId]) ON DELETE NO ACTION ON UPDATE NO ACTION;

