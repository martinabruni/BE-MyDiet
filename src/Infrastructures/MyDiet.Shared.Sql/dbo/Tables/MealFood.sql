CREATE TABLE [dbo].[MealFood] (
    [Id]                  INT             IDENTITY (1, 1) NOT NULL,
    [MealId]              INT             NULL,
    [FoodId]              INT             NULL,
    [Quantity]            DECIMAL (18, 4) NOT NULL,
    [UnitOfMeasurementId] INT             NULL,
    [CreatedAt]           DATETIME        NOT NULL,
    [UpdatedAt]           DATETIME        NULL,
    [UserId] UNIQUEIDENTIFIER NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MealFood_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_MealFood_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_MealFood_UoM] FOREIGN KEY ([UnitOfMeasurementId]) REFERENCES [dbo].[UnitOfMeasurement] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_MealFood_CoreUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser]([Id]) ON DELETE SET NULL
);

