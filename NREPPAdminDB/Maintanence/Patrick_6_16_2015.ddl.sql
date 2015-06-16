GO

ALTER TABLE RC_DocData
ADD ShowReviewer BIT NOT NULL DEFAULT 0

GO

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

ALTER PROCEDURE [dbo].[SPAddOrUpdateDocTags]
	@Id INT = 0,
	@DocId int = 0,
	@DocType int = null,
	@Reference VARCHAR(250),
	@RCName VARCHAR(50),
	@PubYear INT = NULL,
	@AddToReview BIT = 0
AS SET NOCOUNT ON
	BEGIN TRANSACTION

	IF @Id <= 0 BEGIN
		SELECT @Id = Id FROM RC_DocData
		WHERE DocumentId = @DocId
	END

	IF @Id > 0 BEGIN

		UPDATE RC_DocData
		SET Reference = @Reference,
		RCName = @RCName,
		PubYear = @PubYear,
		ShowReviewer = @AddToReview
		WHERE Id = @Id

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		IF @DocID < 1 BEGIN
			SELECT @DocId = DocumentId from RC_DocData
			WHERE Id = @Id
		END

		IF @DocType IS NULL BEGIN
			SELECT @DocType = TypeOfDocument from Document
			WHERE Id = @DocId
		END

		UPDATE Document
		SET TypeOfDocument = @DocType
		WHERE Id = @DocId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END

	END
	ELSE BEGIN

		INSERT INTO RC_DocData (DocumentId, Reference, RCName, PubYear, ShowReviewer) VALUES (@DocId, @Reference, @RCName, @PubYear, @AddToReview)

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -3
		END

		IF @DocType IS NULL BEGIN
			SELECT @DocType = TypeOfDocument from Document
			WHERE Id = @DocId
		END

		UPDATE Document
		SET TypeOfDocument = @DocType
		WHERE Id = @DocId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -4
		END
	END

	COMMIT TRANSACTION
RETURN 0

GO