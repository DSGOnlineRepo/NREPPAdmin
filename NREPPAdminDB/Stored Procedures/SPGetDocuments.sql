﻿CREATE PROCEDURE [dbo].[SPGetDocuments]
	@InvId int = NULL,
	@ReviewerId int = NULL,
	@DocumentId int = NULL
AS
	SELECT d.Id as DocId, Description, FileName, MIME as MIMEType, UploadedBy, u.Firstname + ' ' + u.Lastname as [Uploader], ReviewerId, TypeOfDocument, Title,
	a.LongAnswer as [Document Type Name]
	from Document d
		INNER JOIN AspNetUsers u ON u.Id = d.UploadedBy
		INNER JOIN Answers a ON TypeOfDocument = a.Id
			WHERE
			(@InvId IS NULL OR InterventionId = @InvId)
			AND (@ReviewerId IS NULL OR ReviewerId = @ReviewerId)
			AND (@DocumentId IS NULL OR d.Id = @DocumentId)
RETURN 0
