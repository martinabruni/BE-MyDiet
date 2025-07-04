CREATE TABLE [dbo].[Meal] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [PlanId]     INT      NOT NULL,
    [MealTypeId] INT      NOT NULL,
    [CreatedAt]  DATETIME NOT NULL,
    [UpdatedAt]  DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Meal_MealType] FOREIGN KEY ([MealTypeId]) REFERENCES [dbo].[MealType] ([Id]),
    CONSTRAINT [FK_Meal_Plan] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[Plan] ([Id])
);

