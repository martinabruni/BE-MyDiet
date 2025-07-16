CREATE TABLE [dbo].[CalendarMeal] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [CalendarEntryId] INT      NULL,
    [MealId]          INT      NULL,
    [CreatedAt]       DATETIME NOT NULL,
    [UpdatedAt]       DATETIME NULL,
    [UserId] UNIQUEIDENTIFIER NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_CalendarMeal_CalendarEntry] FOREIGN KEY ([CalendarEntryId]) REFERENCES [dbo].[CalendarEntry] ([Id]),
    CONSTRAINT [FK_CalendarMeal_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal] ([Id]),
    CONSTRAINT [FK_CalendarMeal_CoreUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser]([Id])
);

