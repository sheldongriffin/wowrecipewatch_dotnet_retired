CREATE TABLE [dbo].[Realms] (
    [RealmId]     INT            IDENTITY (1, 1) NOT NULL,
    [Type]        NVARCHAR (50)  NULL,
    [Population]  NVARCHAR (50)  NOT NULL,
    [Queue]       BIT            NOT NULL,
    [Status]      BIT            NOT NULL,
    [Name]        NVARCHAR (100) NOT NULL,
    [Slug]        NVARCHAR (100) NOT NULL,
    [Battlegroup] NVARCHAR (100) NOT NULL
);

