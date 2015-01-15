CREATE PROCEDURE [dbo].[SPGetDocuments]
	@InvId int = NULL,
	@ReviewerId int
AS
	SELECT invd.Id, Description, FileName, MIME as MIMEType, UploadedBy, u.Firstname + ' ' + u.Lastname as [Uploader], ReviewerId, TypeOfDocument,
	a.LongAnswer as [Document Type Name]
	from Document invd
		INNER JOIN Users u ON u.Id = invd.UploadedBy
		INNER JOIN Answers a ON TypeOfDocument = a.Id
			WHERE (@InvId IS NULL OR InterventionId = @InvId) AND (@ReviewerId IS NULL OR ReviewerId = @ReviewerId)
RETURN 0
