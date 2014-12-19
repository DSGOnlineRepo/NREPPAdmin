CREATE TABLE [dbo].[IntervDoc]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [DisplayName] VARCHAR(50) NULL, 
    [FileName] VARCHAR(100) NOT NULL, 
    [MIME] VARCHAR(20) NULL, 
    [InterventionId] INT NOT NULL, 
    [UploadedBy] INT NOT NULL, 
    [TypeOfDocument] INT NULL DEFAULT 1, 
    CONSTRAINT [FK_InvervDoc_Intervention] FOREIGN KEY ([InterventionId]) REFERENCES [Interventions]([Id]), 
    CONSTRAINT [FK_InvervDoc_Users] FOREIGN KEY ([UploadedBy]) REFERENCES [Users]([Id])
)
