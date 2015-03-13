CREATE TABLE [dbo].[OutcomeMeasure]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[OutcomeId] INT NOT NULL,
	[StudyId] INT NOT NULL,
	[OutcomeMeasure] VARCHAR(50) NULL, 
    [BaselineEquiv] INT NULL, 
    [SignificantImpact] INT NULL, 
    [GroupFavored] BIT NULL DEFAULT 0, 
    [PopDescription] VARCHAR(50) NULL, 
    [SAMHSAPop] INT NULL, 
    [PrimaryOutcome] BIT NULL DEFAULT 0, 
    [Priority] INT NULL, 
    [DocId] INT NULL, 
    [RecommendReview] BIT NULL DEFAULT 0, 
    CONSTRAINT [FK_OutcomeMeasure_Outcome] FOREIGN KEY ([OutcomeId]) REFERENCES [Outcome]([Id]), 
    CONSTRAINT [FK_OutcomeMeasure_Study] FOREIGN KEY ([StudyId]) REFERENCES [Studies]([Id]),
	CONSTRAINT [FK_OutcomeMeasure_Document] FOREIGN KEY ([DocId]) REFERENCES [Document]([Id])
)
