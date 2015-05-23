CREATE TABLE [dbo].[Interventions]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Title] VARCHAR(50) NOT NULL, 
    [FullDescription] NTEXT NULL, 
    [SubmitterId] NVARCHAR(128) NOT NULL, 
    [UpdateDate] DATETIME NULL, 
    [PublishDate] DATE NULL, 
    [Status] INT NULL, 
    [RC1] NVARCHAR(128) NULL, 
    [RC2] NVARCHAR(128) NULL, 
    [Reviewer] NVARCHAR(128) NULL, 
    [Acronym] VARCHAR(20) NULL, 
    [ProgramType] INT NULL DEFAULT 0, 
    [Owner] NVARCHAR(128) NULL, 
    [FromListSearch] BIT NOT NULL DEFAULT 0, 
    [PreScreenAnswers] INT NULL DEFAULT 0, 
    [UserPreScreenAnswer] INT NULL, 
    [ScreeningNotes] VARCHAR(MAX) NULL, 
    [MaterialsList] VARCHAR(MAX) NULL, 
    [HaveMaterials] BIT NULL DEFAULT 0, 
    [LitReviewDone] BIT NULL DEFAULT 0, 
    CONSTRAINT [FK_Interventions_InterventionStatus] FOREIGN KEY ([Status]) REFERENCES [InterventionStatus]([Id]) 
)
