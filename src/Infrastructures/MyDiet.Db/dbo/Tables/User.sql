CREATE TABLE [dbo].[User] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Username]       NCHAR (50)       NULL,
    [Email]          NCHAR (255)      NOT NULL,
    [HashedPassword] NCHAR (255)      NOT NULL,
    [CreatedAt]      DATETIME         NOT NULL,
    [UpdatedAt]      DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

