CREATE TABLE [dbo].[Interventions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] VARCHAR(50) NOT NULL, 
    [FullDescription] NTEXT NULL, 
    [Submitter] INT NOT NULL, 
    [UpdateDate] DATETIME NULL, 
    [PublishDate] DATE NULL, 
    [Status] INT NULL, 
    [RC1] INT NULL, 
    [RC2] INT NULL, 
    [Reviewer] INT NULL, 
    [Acronym] VARCHAR(20) NULL, 
    [ProgramType] INT NULL, 
    CONSTRAINT [FK_Interventions_InterventionStatus] FOREIGN KEY ([Status]) REFERENCES [InterventionStatus]([Id]) 
)
