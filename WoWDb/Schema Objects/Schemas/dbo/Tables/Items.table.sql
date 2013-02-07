CREATE TABLE [dbo].[Items] (
    [ItemId]            INT            NOT NULL,
    [Name]              NVARCHAR (MAX) NOT NULL,
    [RecipeId]           INT            NULL,
    [ServerwideAverage] BIGINT         NULL,
    [Quality]           INT            NULL,
    [BindOnPickup]      INT            NULL,
    [SourceId]          INT            NULL,
    [SourceType]        NVARCHAR (100) NULL,
    [Icon]              NVARCHAR (100) NULL
);

