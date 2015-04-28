CREATE TABLE [dbo].[Reviewer_Expertise]
(
	[ReviewerId] NVARCHAR(128) NOT NULL, 
    [AreaOfExpertise] INT NOT NULL, 
    CONSTRAINT [FK_Reviewer_Expertise_Reviewer] FOREIGN KEY ([ReviewerId]) REFERENCES [Reviewers]([Id]), 
    CONSTRAINT [FK_Reviewer_Expertise_Answer] FOREIGN KEY ([AreaOfExpertise]) REFERENCES [Answers]([Id])
)
