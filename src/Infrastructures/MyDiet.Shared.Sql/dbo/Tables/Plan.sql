CREATE TABLE [dbo].[Plan] (
    [Id]        INT        IDENTITY (1, 1) NOT NULL,
    [DietId]    INT        NULL,
    [Name]      NCHAR (50) NOT NULL,
    [CreatedAt] DATETIME   NOT NULL,
    [UpdatedAt] DATETIME   NULL,
    [UserId] UNIQUEIDENTIFIER NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Plan_Diet] FOREIGN KEY ([DietId]) REFERENCES [dbo].[Diet] ([Id]) ON DELETE SET NULL,
    CONSTRAINT [FK_Plan_CoreUser] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser]([Id]) ON DELETE SET NULL
);

