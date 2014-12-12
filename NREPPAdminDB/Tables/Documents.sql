CREATE TABLE [dbo].[Documents]
(
	[Id] [uniqueidentifier] ROWGUIDCOL NOT NULL UNIQUE,
	[SerialNumber] INTEGER UNIQUE,
	[Doc] VARBINARY(MAX) FILESTREAM,
	[InterventionID] INT NOT NULL,
	[UploadedBy] INT NULL, 
    CONSTRAINT [FK_Documents_Intervention] FOREIGN KEY ([InterventionID]) REFERENCES [Interventions]([Id]),
	CONSTRAINT [FK_Documents_UploadedBy] FOREIGN KEY ([UploadedBy]) REFERENCES [Users]([Id])
)
