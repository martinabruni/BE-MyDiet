CREATE TABLE [dbo].[UnitConversion] (
    [Id]               INT             IDENTITY (1, 1) NOT NULL,
    [FromUnitId]       INT             NULL,
    [ToUnitId]         INT             NULL,
    [ConversionFactor] DECIMAL (18, 4) NOT NULL,
    [CreatedAt]        DATETIME        NOT NULL,
    [UpdatedAt]        DATETIME        NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UnitConversion_FromUoM] FOREIGN KEY ([FromUnitId]) REFERENCES [dbo].[UnitOfMeasurement] ([Id]),
    CONSTRAINT [FK_UnitConversion_ToUoM] FOREIGN KEY ([ToUnitId]) REFERENCES [dbo].[UnitOfMeasurement] ([Id])
);

