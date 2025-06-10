## Entities

```sql
-- User\CREATE TABLE [dbo].[User]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [Username] NCHAR(50) NULL,
    [Email] NCHAR(255) NOT NULL,
    [HashedPassword] NCHAR(255) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL
);

-- Diet\CREATE TABLE [dbo].[Diet]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [Name] NCHAR(50) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_Diet_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id])
);

-- Plan\CREATE TABLE [dbo].[Plan]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [DietId] INT NOT NULL,
    [Name] NCHAR(50) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_Plan_Diet] FOREIGN KEY ([DietId]) REFERENCES [dbo].[Diet]([Id])
);

-- MealType\CREATE TABLE [dbo].[MealType]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NCHAR(50) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL
);

-- Meal\CREATE TABLE [dbo].[Meal]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [PlanId] INT NOT NULL,
    [MealTypeId] INT NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_Meal_Plan] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[Plan]([Id]),
    CONSTRAINT [FK_Meal_MealType] FOREIGN KEY ([MealTypeId]) REFERENCES [dbo].[MealType]([Id])
);

-- Food\CREATE TABLE [dbo].[Food]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NCHAR(50) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL
);

-- UnitOfMeasurement\CREATE TABLE [dbo].[UnitOfMeasurement]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Name] NCHAR(50) NOT NULL,
    [Abbreviation] NCHAR(10) NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL
);

-- MealFood\CREATE TABLE [dbo].[MealFood]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [MealId] INT NOT NULL,
    [FoodId] INT NOT NULL,
    [Quantity] DECIMAL(18,4) NOT NULL,
    [UnitOfMeasurementId] INT NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_MealFood_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal]([Id]),
    CONSTRAINT [FK_MealFood_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food]([Id]),
    CONSTRAINT [FK_MealFood_UoM] FOREIGN KEY ([UnitOfMeasurementId]) REFERENCES [dbo].[UnitOfMeasurement]([Id])
);

-- CalendarEntry\CREATE TABLE [dbo].[CalendarEntry]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [PlanId] INT NOT NULL,
    [Date] DATE NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_CalendarEntry_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User]([Id]),
    CONSTRAINT [FK_CalendarEntry_Plan] FOREIGN KEY ([PlanId]) REFERENCES [dbo].[Plan]([Id])
);

-- CalendarMeal\CREATE TABLE [dbo].[CalendarMeal]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CalendarEntryId] INT NOT NULL,
    [MealId] INT NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_CalendarMeal_CalendarEntry] FOREIGN KEY ([CalendarEntryId]) REFERENCES [dbo].[CalendarEntry]([Id]),
    CONSTRAINT [FK_CalendarMeal_Meal] FOREIGN KEY ([MealId]) REFERENCES [dbo].[Meal]([Id])
);

-- FoodAlternative\CREATE TABLE [dbo].[FoodAlternative]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [FoodId] INT NOT NULL,
    [AlternativeFoodId] INT NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_FoodAlternative_Food] FOREIGN KEY ([FoodId]) REFERENCES [dbo].[Food]([Id]),
    CONSTRAINT [FK_FoodAlternative_AltFood] FOREIGN KEY ([AlternativeFoodId]) REFERENCES [dbo].[Food]([Id])
);

-- MealSwap\CREATE TABLE [dbo].[MealSwap]
(
    [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [CalendarMealId] INT NOT NULL,
    [SwappedCalendarMealId] INT NOT NULL,
    [CreatedAt] DATETIME NULL,
    [UpdatedAt] DATETIME NULL,
    CONSTRAINT [FK_MealSwap_CalendarMeal] FOREIGN KEY ([CalendarMealId]) REFERENCES [dbo].[CalendarMeal]([Id]),
    CONSTRAINT [FK_MealSwap_Swapped] FOREIGN KEY ([SwappedCalendarMealId]) REFERENCES [dbo].[CalendarMeal]([Id])
);

-- UnitConversion\CREATE TABLE [dbo].[UnitConversion]
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
```

## Entity Relationships

* **User** 1\:N **Diet**
* **Diet** 1\:N **Plan**
* **Plan** 1\:N **Meal**
* **MealType** 1\:N **Meal**
* **Meal** 1\:N **MealFood**
* **Food** 1\:N **MealFood**
* **UnitOfMeasurement** 1\:N **MealFood**
* **User** 1\:N **CalendarEntry**
* **Plan** 1\:N **CalendarEntry**
* **CalendarEntry** 1\:N **CalendarMeal**
* **CalendarMeal** 1\:N **MealSwap** (via `calendar_meal_id`)
* **CalendarMeal** 1\:N **MealSwap** (via `swapped_calendar_meal_id`)
* **Food** 1\:N **FoodAlternative** (via `food_id`)
* **Food** 1\:N **FoodAlternative** (via `alternative_food_id`)
* **UnitOfMeasurement** 1\:N **UnitConversion** (via `from_unit_id`)
* **UnitOfMeasurement** 1\:N **UnitConversion** (via `to_unit_id`)