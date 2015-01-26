CREATE TABLE [dbo].[Document_Outcome]
(
	[StudyId] INT NOT NULL, 
    [OutcomeId] INT NOT NULL, 
	[DocumentId] INT NULL, 
    CONSTRAINT [FK_Document_Outcome_Outcome] FOREIGN KEY ([OutcomeId]) REFERENCES [Outcome]([Id])
)
