CREATE PROCEDURE [dbo].[SPAddOrUpdateDocTags]
	@Id INT = 0,
	@DocId int = 0,
	@DocType int = null,
	@Reference VARCHAR(250),
	@RCName VARCHAR(50),
	@PubYear INT = NULL
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
		PubYear = @PubYear
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

		INSERT INTO RC_DocData (DocumentId, Reference, RCName, PubYear) VALUES (@DocId, @Reference, @RCName, @PubYear)

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
