CREATE TABLE [dbo].[Recipes] (
    [RecipeId]      INT            NOT NULL,
    [Name]          NVARCHAR (200) NULL,
    [ItemId]		INT            NULL,
    [TradeskillId]  INT            NOT NULL,
    [Source]        NVARCHAR (MAX) NULL,
	[ServerwideAverage] BIGINT     NULL,
	[CurrentPrice]  BIGINT         NULL
);

