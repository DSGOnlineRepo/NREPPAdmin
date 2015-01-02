CREATE TABLE [dbo].[Outcome]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [StudyId] INT NULL, 
    [OutcomeMeasure] VARCHAR(50) NULL, 
    [Citation] VARCHAR(60) NULL, 
    [OverallAttrition] BIT NOT NULL DEFAULT 0, 
    [DiffAttrition] BIT NULL DEFAULT 0, 
    [EffectSize] BIT NULL DEFAULT 0, 
    [BaselineEquiv] INT NULL, 
    [SignificantImpact] INT NULL, 
    [GroupFavored] BIT NULL DEFAULT 0, 
    [PopDescription] VARCHAR(50) NULL, 
    [SAMHSAPop] INT NULL, 
    [PrimaryOutcome] BIT NULL DEFAULT 0, 
    [Priority] INT NULL
)
