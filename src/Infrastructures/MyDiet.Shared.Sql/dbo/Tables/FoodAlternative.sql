CREATE TABLE [dbo].[FoodAlternative] (
    [Id]                INT      IDENTITY (1, 1) NOT NULL,
    [FoodId]            INT      NULL,
    [AlternativeFoodId] INT      NULL,
    [CreatedAt]         DATETIME NOT NULL,
    [UpdatedAt]         DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FoodAlternative_AltFood] FOREIGN KEY ([AlternativeFoodId]) REFERENCES [dbo].[Food] ([Id]),
    CONSTRAINT [FK_FoodAlternative_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food] ([Id])
);

