CREATE PROCEDURE [dbo].[SPGetInterventionList]
	@param1 int = 0,
	@param2 int
AS
	SELECT TOP 100 i.Id as InterventionId, Title, FullDescription, u.FirstName + ' ' + u.LastName as [Submitter] from Interventions i
	inner join Users u ON i.Submitter = u.Id
		
RETURN 0
