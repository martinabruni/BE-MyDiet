CREATE TABLE [dbo].[UnitConversion]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [FromUnitId] INT NOT NULL,
    [ToUnitId] INT NOT NULL,
    [ConversionFactor] DECIMAL(18,4) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_UnitConversion_FromUoM] FOREIGN KEY ([FromUnitId]) REFERENCES [dbo].[UnitOfMeasurement]([Id]),
    CONSTRAINT [FK_UnitConversion_ToUoM] FOREIGN KEY ([ToUnitId]) REFERENCES [dbo].[UnitOfMeasurement]([Id])
);
