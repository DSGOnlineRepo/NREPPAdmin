CREATE TABLE [dbo].[Document]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Description] VARCHAR(50) NULL, 
    [FileName] VARCHAR(100) NOT NULL, 
    [MIME] VARCHAR(20) NULL, 
    [InterventionId] INT NULL, 
    [UploadedBy] nvarchar(128) NOT NULL, 
    [TypeOfDocument] INT NULL DEFAULT 1, 
    [ReviewerId] NVARCHAR(128) NULL, 
    [IsLitSearch] BIT NOT NULL DEFAULT 0, 
    [ReviewerName] VARCHAR(100) NULL, 
    CONSTRAINT [FK_InvervDoc_Intervention] FOREIGN KEY ([InterventionId]) REFERENCES [Interventions]([Id]), 
    CONSTRAINT [FK_InvervDoc_Users] FOREIGN KEY ([UploadedBy]) REFERENCES [AspNetUsers]([Id])
)
