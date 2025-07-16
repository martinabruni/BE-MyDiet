CREATE TABLE [dbo].[Meal] (
    [Id]         INT      IDENTITY (1, 1) NOT NULL,
    [PlanId]     INT      NULL,
    [MealTypeId] INT      NULL,
    [CreatedAt]  DATETIME NOT NULL,
    [UpdatedAt]  DATETIME NULL,
    [UserId] UNIQUEIDENTIFIER NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Meal_MealType] FOREIGN KEY ([MealTypeId]) REFERENCES [dbo].[MealType] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Meal_Plan] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[Plan] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Meal_CoreUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser]([Id]) ON DELETE SET NULL
);

