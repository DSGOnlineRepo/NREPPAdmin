CREATE PROCEDURE [dbo].[SPAddOrRemoveDoc]
	@IntervId INT= NULL,
	@ReviewerID INT = NULL,
	@UploaderId INT,
	@Description VARCHAR(50) = NULL,
	@FileName VARCHAR(100) = NULL,
	@MIMEType VARCHAR(20) = NULL,
	@IsDelete BIT = 0,
	@ItemId INT = -1,
	@DocumentTypeID INT = 4,
	@Output INT = -1 OUTPUT,
	@ReviewerName VARCHAR(50) = NULL,
	@IsLitSearch BIT = 0
AS
	BEGIN TRANSACTION

		--IF @ReviewerID IS NULL AND @I
		IF @IsDelete = 1 BEGIN
			DELETE FROM Document
			WHERE Id = @ItemId

			IF @@ERROR <> 0 BEGIN
				ROLLBACK TRANSACTION
				RETURN -1
			END
		END
		ELSE BEGIN
			INSERT INTO Document VALUES (@Description, @FileName, @MIMEType, @IntervId, @UploaderId, @DocumentTypeID,
				@ReviewerID, @IsLitSearch, @ReviewerName)

			SELECT @Output = @@IDENTITY

			IF @@ERROR <> 0 BEGIN
				ROLLBACK TRANSACTION
				RETURN -2
			END
		END

	COMMIT TRANSACTION
RETURN 0
