CREATE TABLE [dbo].[OutcomeMeasure]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[OutcomeId] INT NOT NULL,
	[StudyId] INT NOT NULL,
	[OutcomeMeasure] VARCHAR(50) NULL, 
    [OverallAttrition] BIT NOT NULL DEFAULT 0, 
    [DiffAttrition] BIT NULL DEFAULT 0, 
    [EffectSize] BIT NULL DEFAULT 0, 
    [BaselineEquiv] INT NULL, 
    [SignificantImpact] INT NULL, 
    [GroupFavored] BIT NULL DEFAULT 0, 
    [PopDescription] VARCHAR(50) NULL, 
    [SAMHSAPop] INT NULL, 
    [PrimaryOutcome] BIT NULL DEFAULT 0, 
    [Priority] INT NULL, 
    CONSTRAINT [FK_OutcomeMeasure_Outcome] FOREIGN KEY ([OutcomeId]) REFERENCES [Outcome]([Id]), 
    CONSTRAINT [FK_OutcomeMeasure_Study] FOREIGN KEY ([StudyId]) REFERENCES [Studies]([Id])
)
