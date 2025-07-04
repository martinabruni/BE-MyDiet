CREATE TABLE [dbo].[UnitOfMeasurement] (
    [Id]           INT        IDENTITY (1, 1) NOT NULL,
    [Name]         NCHAR (50) NOT NULL,
    [Abbreviation] NCHAR (10) NOT NULL,
    [CreatedAt]    DATETIME   NOT NULL,
    [UpdatedAt]    DATETIME   NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

