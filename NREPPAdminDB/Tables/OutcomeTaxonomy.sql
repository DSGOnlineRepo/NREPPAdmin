CREATE TABLE [dbo].[OutcomeTaxonomy]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [OutcomeName] VARCHAR(MAX) NULL, 
    [Guidelines] VARCHAR(MAX) NULL, 
    [OutcomeGroupId] INT NULL, 
    [SearchCategories] VARCHAR(MAX) NULL, 
    [Measures] VARCHAR(MAX) NULL, 
    [IsPrimary] BIT NOT NULL DEFAULT 0
)
