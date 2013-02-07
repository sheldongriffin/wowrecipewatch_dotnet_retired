ALTER TABLE [dbo].[Auctions]
    ADD CONSTRAINT [PK_Auctions] PRIMARY KEY CLUSTERED ([RealmId] ASC, [AuctionId] ASC) WITH (ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);
