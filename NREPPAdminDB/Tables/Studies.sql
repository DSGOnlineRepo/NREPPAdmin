CREATE TABLE [dbo].[Studies]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[StudyId] INT NOT NULL,
    [Reference] VARCHAR(100) NULL, 
    [FromLitSearch] BIT NOT NULL DEFAULT 0, 
    [Exclusion1] INT NULL, 
    [Exclusion2] INT NULL, 
    [Exclusion3] INT NULL, 
    [StudyDesign] INT NULL, 
    [GroupSize] VARCHAR(200) NULL, 
    [UseMultivariate] BIT NOT NULL DEFAULT 0,
	[TotalSampleSize] VARCHAR(200) NULL,
    [LongestFollowup] VARCHAR(200) NULL, 
    [SAMHSARelated] INT NULL, 
    [AuthorQueryNeeded] BIT NOT NULL DEFAULT 0, 
    [RecommendReview] BIT NOT NULL DEFAULT 0, 
    [Notes] VARCHAR(MAX) NULL, 
    [DocumentId] INT NOT NULL, 
    [OverallAttritionAvail] INT NULL, 
    [DiffAttritionAvail] INT NULL DEFAULT 1, 
    [DocOrdinal] INT NULL DEFAULT 1, 
    CONSTRAINT [FK_Studies_Document] FOREIGN KEY ([DocumentId]) REFERENCES [Document]([Id])
)
