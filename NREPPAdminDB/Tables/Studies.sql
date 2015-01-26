CREATE TABLE [dbo].[Studies]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[StudyId] INT NOT NULL,
    [Reference] VARCHAR(100) NULL, 
    [InLitSearch] BIT NOT NULL DEFAULT 0, 
    [Exclusion1] INT NULL, 
    [Exclusion2] INT NULL, 
    [Exclusion3] INT NULL, 
    [StudyDesign] INT NULL, 
    [GroupSize] NCHAR(10) NULL, 
    [BaselineEquiv] VARCHAR(MAX) NULL, 
    [UseMultivariate] BIT NOT NULL DEFAULT 0, 
    [LongestFollowup] NCHAR(10) NULL, 
    [SAMSHARelated] INT NULL, 
    [TargetPop] NCHAR(10) NULL, 
    [ListOfOutcomes] NCHAR(10) NULL, 
    [AuthorQueryNeeded] BIT NOT NULL DEFAULT 0, 
    [RecommendReview] BIT NOT NULL DEFAULT 0, 
    [Notes] VARCHAR(MAX) NULL, 
    [DocumentId] INT NOT NULL, 
    CONSTRAINT [FK_Studies_Document] FOREIGN KEY ([DocumentId]) REFERENCES [Document]([Id])
)
