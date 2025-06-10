CREATE TABLE [dbo].[UnitOfMeasurement]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NCHAR(50) NOT NULL,
    [Abbreviation] NCHAR(10) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL
);
