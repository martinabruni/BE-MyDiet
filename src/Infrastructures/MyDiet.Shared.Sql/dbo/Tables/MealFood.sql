CREATE TABLE [dbo].[MealFood] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [MealId]              INT             NOT NULL,
    [FoodId]              INT             NOT NULL,
    [Quantity]            DECIMAL (18, 4) NOT NULL,
    [UnitOfMeasurementId] INT             NOT NULL,
    [CreatedAt]           DATETIME        NOT NULL,
    [UpdatedAt]           DATETIME        NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MealFood_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food] ([Id]),
    CONSTRAINT [FK_MealFood_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal] ([Id]),
    CONSTRAINT [FK_MealFood_UoM] FOREIGN KEY ([UnitOfMeasurementId]) REFERENCES [dbo].[UnitOfMeasurement] ([Id]),
    CONSTRAINT [FK_MealFood_CoreUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser]([Id])
);

