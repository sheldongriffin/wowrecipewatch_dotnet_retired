CREATE TABLE [dbo].[Characters] (
    [CharacterId]       INT            IDENTITY (1, 1) NOT NULL,
    [LastModified]      BIGINT         NOT NULL,
    [Name]              NVARCHAR (MAX) NOT NULL,
    [RealmId]           INT            NOT NULL,
	[RaceId]            INT            NOT NULL,
    [ClassId]           INT            NOT NULL,
    [Gender]            INT            NOT NULL,
    [Level]             INT            NOT NULL,
    [AchievementPoints] INT            NOT NULL,
    [Thumbnail]         NVARCHAR (MAX) NOT NULL
);

