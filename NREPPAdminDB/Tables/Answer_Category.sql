CREATE TABLE [dbo].[Answer_Category]
(
	AnswerID INT NOT NULL,
	CategoryID INT NOT NULL, 
    CONSTRAINT [FK_Answer_Category_Answers] FOREIGN KEY ([AnswerID]) REFERENCES [Answers]([Id]), 
    CONSTRAINT [FK_Answer_Category_Category] FOREIGN KEY ([CategoryID]) REFERENCES [Category]([Id])
)
