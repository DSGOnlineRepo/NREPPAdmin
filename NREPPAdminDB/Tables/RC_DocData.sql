CREATE TABLE [dbo].[RC_DocData]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY, 
    [DocumentId] INT not NULL, 
    [Reference] VARCHAR(250) NULL, 
    [RCName] VARCHAR(50) NULL, 
    CONSTRAINT [FK_RC_DocData_ToTable] FOREIGN KEY ([DocumentId]) REFERENCES [Document]([Id])
)
