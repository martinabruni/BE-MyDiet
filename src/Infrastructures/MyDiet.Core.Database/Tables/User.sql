CREATE TABLE [dbo].[User]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Username] NCHAR(50) NULL, 
    [Email] NCHAR(255) NOT NULL, 
    [HashedPassword] NCHAR(255) NOT NULL, 
    [CreatedAt] DATETIME NULL, 
    [UpdateAt] DATETIME NULL
)
