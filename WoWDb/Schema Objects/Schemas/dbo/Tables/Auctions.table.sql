CREATE TABLE [dbo].[Auctions] (
    [RealmId]      INT            NOT NULL,
    [Faction]      NVARCHAR (100) NOT NULL,
    [AuctionId]    BIGINT         NOT NULL,
    [ItemId]       INT            NOT NULL,
    [Owner]        NVARCHAR (100) NOT NULL,
    [Bid]          BIGINT         NULL,
    [Buyout]       BIGINT         NULL,
    [Quantity]     BIGINT         NOT NULL,
    [TimeLeft]     NVARCHAR (50)  NOT NULL,
    [LastModified] BIGINT         NOT NULL
);

