CREATE TABLE [dbo].[SAMHSAGroup]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [GroupDescription] VARCHAR(MAX) NULL, 
    [DoReview] BIT NOT NULL DEFAULT 0
)
