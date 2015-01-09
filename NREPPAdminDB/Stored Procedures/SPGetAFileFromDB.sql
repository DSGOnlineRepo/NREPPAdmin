CREATE PROCEDURE [dbo].[SPGetAFileFromDB]
	@FileId INT = NULL,
	@InterVId INT = NULL
AS
	IF @FileId IS NOT NULL BEGIN
		SELECT Id as FileId, Description, FileName, MIME, InterventionId, UploadedBy as [Uploader], TypeOfDocument FROM Document
		WHERE Id = @FileId
	END
	ELSE BEGIN
		SELECT Id as FileId, Description, FileName, MIME, InterventionId, UploadedBy as [Uploader], TypeOfDocument FROM Document
		WHERE InterventionId = @InterVId
	END

RETURN 0
