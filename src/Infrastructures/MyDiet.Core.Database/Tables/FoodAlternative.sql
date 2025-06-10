CREATE TABLE [dbo].[FoodAlternative]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [FoodId] INT NOT NULL,
    [AlternativeFoodId] INT NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_FoodAlternative_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food]([Id]),
    CONSTRAINT [FK_FoodAlternative_AltFood] FOREIGN KEY ([AlternativeFoodId]) REFERENCES [dbo].[Food]([Id])
);
