CREATE TABLE [dbo].[AuthUser] (
    [Id]             UNIQUEIDENTIFIER NOT NULL,
    [Username]       NCHAR (50)       NULL,
    [Email]          NCHAR (255)      NOT NULL,
    [HashedPassword] NCHAR (255)      NOT NULL,
    [Role] INT NOT NULL DEFAULT 0, 
    [CreatedAt]      DATETIME         NOT NULL,
    [UpdatedAt]      DATETIME         NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC) 
);

