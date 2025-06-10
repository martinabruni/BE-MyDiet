CREATE TABLE [dbo].[MealFood]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [MealId] INT NOT NULL,
    [FoodId] INT NOT NULL,
    [Quantity] DECIMAL(18,4) NOT NULL,
    [UnitOfMeasurementId] INT NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_MealFood_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal]([Id]),
    CONSTRAINT [FK_MealFood_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food]([Id]),
    CONSTRAINT [FK_MealFood_UoM] FOREIGN KEY ([UnitOfMeasurementId]) REFERENCES [dbo].[UnitOfMeasurement]([Id])
);
