﻿CREATE PROCEDURE [dbo].[SPGetOutcomesByInterventionId]
	@InterventionId INT
AS

	DECLARE  @Docs TABLE (DocumentId int)

	INSERT INTO @Docs (DocumentId) select Id from Document where InterventionId = @InterventionId

	SELECT o.Id as [OutcomeMeasureId], OutcomeMeasure, BaselineEquiv,
		SignificantImpact, GroupFavored, PopDescription, SAMHSAPop, PrimaryOutcome, Priority, StudyId, DocId as DocumentId, o.Id as OutcomeId from OutcomeMeasure om
	INNER JOIN Outcome o ON o.Id = om.OutcomeId
	WHERE InterventionId = @InterventionId

	SELECT Id, OutcomeName from Outcome 
	WHERE InterventionId = @InterventionId

	SELECT DocumentId, StudyId, OutcomeId FROM Document_Outcome
	where DocumentId in (SELECT DocumentId from @Docs)

RETURN 0
