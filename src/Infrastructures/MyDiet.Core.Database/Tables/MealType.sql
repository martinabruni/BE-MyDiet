CREATE TABLE [dbo].[MealType]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NCHAR(50) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL
);
