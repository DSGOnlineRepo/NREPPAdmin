CREATE TABLE [dbo].[Document_Study]
(
	[DocumentId] INT NOT NULL,
	[StudyId] INT NOT NULL, 
    [DocumentType] INT NULL, 
    CONSTRAINT [FK_Document_Study_ToDocument] FOREIGN KEY ([DocumentId]) REFERENCES [Document]([Id]),
	CONSTRAINT [FK_Document_Study_ToStudy] FOREIGN KEY ([StudyId]) REFERENCES [Studies]([Id])
)
