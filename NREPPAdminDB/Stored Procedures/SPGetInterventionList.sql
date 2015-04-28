CREATE PROCEDURE [dbo].[SPGetInterventionList]
@Id INT = NULL,
@RoleName nvarchar(128) = NULL
AS
SET NOCOUNT ON

	DECLARE @AvailStatus table (statusId INT)

	INSERT INTO @AvailStatus SELECT statusId from FNGetStatusesByRole(@RoleName)

	IF @Id IS NULL BEGIN
		SELECT TOP 100 i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE s.Id in (SELECT statusId from @AvailStatus)

	END
	ELSE BEGIN
		SELECT i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id
	END
		
RETURN 0
