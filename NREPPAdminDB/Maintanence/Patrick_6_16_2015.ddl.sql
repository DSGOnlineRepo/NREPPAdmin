GO

ALTER PROCEDURE [dbo].[SPGetDocsWithTagsById]
	@DocId int = NULL,
	@InterventionId int = NULL
AS

	SELECT d.Id as DocumentId, Description, TypeOfDocument, Reference, RCName, rc.Id as [RCId], PubYear, a.LongAnswer as [DocTypeName], rc.ShowReviewer as AddToReview
	from Document d
	LEFT JOIN RC_DocData rc
	ON rc.DocumentId = d.Id
	INNER JOIN Answers a ON TypeOfDocument = a.Id
	WHERE (@DocId IS NULL OR d.Id = @DocId) AND (@InterventionId IS NULL OR InterventionId = @InterventionId)

RETURN 0

GO

ALTER TABLE RC_DocData
ADD ShowReviewer BIT NOT NULL DEFAULT 0

GO