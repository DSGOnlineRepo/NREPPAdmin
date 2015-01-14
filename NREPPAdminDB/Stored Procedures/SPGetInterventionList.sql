CREATE PROCEDURE [dbo].[SPGetInterventionList]
@Id INT = NULL
AS
SET NOCOUNT ON

	IF @Id IS NULL BEGIN
		SELECT TOP 100 i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.Submitter as [SubmitterId], StatusName, s.Id as [StatusId], PublishDate, UpdateDate, ProgramType
		from Interventions i 
		inner join Users u ON i.Submitter = u.Id
		inner join InterventionStatus s on i.Status = s.Id

		SELECT 1 FROM Document
	END
	ELSE BEGIN
		SELECT i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.Submitter as [SubmitterId], StatusName, s.Id as [StatusId], PublishDate, UpdateDate, ProgramType
		from Interventions i 
		inner join Users u ON i.Submitter = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id

		SELECT invd.Id, Description, FileName, MIME as MIMEType, UploadedBy, u.Firstname + ' ' + u.Lastname as [Uploader] from Document invd
		INNER JOIN Users u ON u.Id = invd.UploadedBy
			WHERE InterventionId = @Id
	END
		
RETURN 0
