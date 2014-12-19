CREATE PROCEDURE [dbo].[SPAddOrRemoveDoc]
	@IntervId INT,
	@UploaderId INT,
	@DisplayName VARCHAR(50) = NULL,
	@FileName VARCHAR(100) = NULL,
	@MIMEType VARCHAR(20) = NULL,
	@IsDelete BIT = 0,
	@ItemId INT = -1
AS
	BEGIN TRANSACTION
		IF @IsDelete = 1 BEGIN
			DELETE FROM IntervDoc
			WHERE Id = @ItemId

			IF @@ERROR <> 0 BEGIN
				ROLLBACK TRANSACTION
				RETURN -1
			END
		END
		ELSE BEGIN
			INSERT INTO IntervDoc VALUES (@DisplayName, @FileName, @MIMEType, @IntervId, @UploaderId)

			IF @@ERROR <> 0 BEGIN
				ROLLBACK TRANSACTION
				RETURN -2
			END
		END

	COMMIT TRANSACTION
RETURN 0
