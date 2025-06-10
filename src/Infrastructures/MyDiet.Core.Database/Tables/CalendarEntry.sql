CREATE TABLE [dbo].[CalendarEntry]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [PlanId] INT NOT NULL,
    [Date] DATE NOT NULL,
    [CreatedAt] DATETIME NOT NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_CalendarEntry_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]),
    CONSTRAINT [FK_CalendarEntry_Plan] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[Plan]([Id])
);
