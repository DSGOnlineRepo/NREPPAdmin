CREATE TABLE [dbo].[OutcomeMeasure]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[OutcomeId] INT NOT NULL,
	[StudyId] INT NOT NULL,
	[OutcomeMeasure] VARCHAR(50) NULL, 
    [GroupFavored] BIT NULL DEFAULT 0, 
    [PopDescription] VARCHAR(50) NULL, 
    [SAMHSAPop] INT NULL,
	[SAMHSAOutcome] INT NULL,
	[EffectReport] INT NULL,
    [DocId] INT NULL, 
    [RecommendReview] BIT NULL DEFAULT 0, 
    [TaxonomyOutcome] INT NULL, 
    CONSTRAINT [FK_OutcomeMeasure_Outcome] FOREIGN KEY ([OutcomeId]) REFERENCES [Outcome]([Id]), 
    CONSTRAINT [FK_OutcomeMeasure_Study] FOREIGN KEY ([StudyId]) REFERENCES [Studies]([Id]),
	CONSTRAINT [FK_OutcomeMeasure_Document] FOREIGN KEY ([DocId]) REFERENCES [Document]([Id]), 
    CONSTRAINT [FK_OutcomeMeasure_TaxOutcomes] FOREIGN KEY ([TaxonomyOutcome]) REFERENCES [OutcomeTaxonomy]([Id])
)
