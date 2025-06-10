CREATE TABLE [dbo].[Plan]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [DietId] INT NOT NULL,
    [Name] NCHAR(50) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_Plan_Diet] FOREIGN KEY ([DietId]) REFERENCES [dbo].[Diet]([Id])
);
