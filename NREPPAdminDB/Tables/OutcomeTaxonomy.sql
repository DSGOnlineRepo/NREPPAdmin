CREATE TABLE [dbo].[OutcomeTaxonomy]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [OutcomeName] VARCHAR(50) NULL, 
    [Guidelines] VARCHAR(MAX) NULL
)
