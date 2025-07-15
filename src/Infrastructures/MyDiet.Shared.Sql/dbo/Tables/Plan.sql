CREATE TABLE [dbo].[Plan] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
    [DietId]    INT        NOT NULL,
    [Name]      NCHAR (50) NOT NULL,
    [CreatedAt] DATETIME   NOT NULL,
    [UpdatedAt] DATETIME   NULL,
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Plan_Diet] FOREIGN KEY ([DietId]) REFERENCES [dbo].[Diet] ([Id]),
    CONSTRAINT [FK_Plan_CoreUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser]([Id])
);

