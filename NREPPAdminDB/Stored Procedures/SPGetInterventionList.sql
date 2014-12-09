CREATE PROCEDURE [dbo].[SPGetInterventionList]
AS
	SELECT TOP 100 i.Id as InterventionId, Title, FullDescription, u.FirstName + ' ' + u.LastName as [Submitter], StatusName, s.Id as [StatusId], PublishDate, UpdateDate from Interventions i 
	inner join Users u ON i.Submitter = u.Id
	inner join InterventionStatus s on i.Status = s.Id
		
RETURN 0
