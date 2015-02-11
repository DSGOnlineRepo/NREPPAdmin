CREATE PROCEDURE [dbo].[SPGetDocsWithTagsById]
	@DocId int = NULL,
	@InterventionId int = NULL
AS

	SELECT d.Id as DocumentId, Description, TypeOfDocument, Reference, RCName, rc.Id as [RCId] from Document d
	LEFT JOIN RC_DocData rc
	ON rc.DocumentId = d.Id
	WHERE (@DocId IS NULL OR d.Id = @DocId) AND (@InterventionId IS NULL OR InterventionId = @InterventionId)

RETURN 0
