CREATE TABLE [dbo].[MealSwap] (
    [Id]                    INT      IDENTITY (1, 1) NOT NULL,
    [CalendarMealId]        INT      NOT NULL,
    [SwappedCalendarMealId] INT      NOT NULL,
    [CreatedAt]             DATETIME NOT NULL,
    [UpdatedAt]             DATETIME NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_MealSwap_CalendarMeal] FOREIGN KEY ([CalendarMealId]) REFERENCES [dbo].[CalendarMeal] ([Id]),
    CONSTRAINT [FK_MealSwap_Swapped] FOREIGN KEY ([SwappedCalendarMealId]) REFERENCES [dbo].[CalendarMeal] ([Id])
);

