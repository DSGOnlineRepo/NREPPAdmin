CREATE TABLE [dbo].[Interventions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] VARCHAR(50) NOT NULL, 
    [FullDescription] NTEXT NULL, 
    [Submitter] INT NOT NULL, 
    [UpdateDate] DATETIME NULL, 
    [PublishDate] DATE NULL, 
    [Status] INT NULL, 
    CONSTRAINT [FK_Interventions_Users] FOREIGN KEY ([Submitter]) REFERENCES [Users]([Id]), 
    CONSTRAINT [FK_Interventions_InterventionStatus] FOREIGN KEY ([Status]) REFERENCES [InterventionStatus]([Id]) 
)
