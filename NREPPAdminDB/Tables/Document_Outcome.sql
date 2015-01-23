CREATE TABLE [dbo].[Document_Outcome]
(
	[DocumentId] INT NOT NULL, 
    [OutcomeId] INT NOT NULL, 
    CONSTRAINT [FK_Document_Outcome_Document] FOREIGN KEY ([DocumentId]) REFERENCES [Document]([Id]),
	CONSTRAINT [FK_Document_Outcome_Outcome] FOREIGN KEY ([OutcomeId]) REFERENCES [Outcome]([Id])
)
