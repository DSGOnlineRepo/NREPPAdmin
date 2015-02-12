CREATE TABLE [dbo].[Outcome]
(
	[Id] INT NOT NULL IDENTITY PRIMARY KEY, 
    [OutcomeName] VARCHAR(30),
	[InterventionId] INT NOT NULL, 
    CONSTRAINT [FK_Outcome_ToTable] FOREIGN KEY ([InterventionId]) REFERENCES [Interventions]([Id])
)
