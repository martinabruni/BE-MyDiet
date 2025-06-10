CREATE TABLE [dbo].[CalendarMeal]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CalendarEntryId] INT NOT NULL,
    [MealId] INT NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_CalendarMeal_CalendarEntry] FOREIGN KEY ([CalendarEntryId]) REFERENCES [dbo].[CalendarEntry]([Id]),
    CONSTRAINT [FK_CalendarMeal_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal]([Id])
);
