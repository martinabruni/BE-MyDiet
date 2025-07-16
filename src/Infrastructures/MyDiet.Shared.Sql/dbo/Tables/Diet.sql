CREATE TABLE [dbo].[Diet] (
    [Id]        INT              IDENTITY (1, 1) NOT NULL,
    [UserId]    UNIQUEIDENTIFIER NULL,
    [Name]      NCHAR (50)       NOT NULL,
    [CreatedAt] DATETIME         NOT NULL,
    [UpdatedAt] DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Diet_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[CoreUser] ([Id]) ON DELETE SET NULL
);

