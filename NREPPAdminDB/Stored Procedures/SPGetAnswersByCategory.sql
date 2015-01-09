CREATE PROCEDURE [dbo].[SPGetAnswersByCategory]
	@InCategory INT = NULL,
	@InCategoryName VARCHAR(15) = NULL
AS SET NOCOUNT ON
	
	SELECT a.Id as [AnswerId], ShortAnswer, LongAnswer from Answers a INNER JOIN
	Answer_Category ac ON a.Id = ac.AnswerID
	INNER JOIN Category c ON c.Id = ac.CategoryID
	WHERE (@InCategory IS NULL OR ac.CategoryID = @InCategory) AND
	(@InCategoryName IS NULL OR c.CategoryName = @InCategoryName)
	--ac.CategoryID = @InCategory

RETURN 0
