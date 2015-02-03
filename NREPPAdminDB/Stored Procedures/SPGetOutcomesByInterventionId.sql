CREATE PROCEDURE [dbo].[SPGetOutcomesByInterventionId]
	@InterventionId INT
AS

	DECLARE  @Docs TABLE (DocumentId int)

	INSERT INTO @Docs (DocumentId) select Id from Document where InterventionId = @InterventionId

	SELECT o.Id as [OutcomeId], OutcomeMeasure, Citation, OverallAttrition, DiffAttrition, EffectSize, BaselineEquiv,
		SignificantImpact, GroupFavored, PopDescription, SAMHSAPop, PrimaryOutcome, Priority, DocumentId from Outcome o
	INNER JOIN Document_Outcome do on o.Id = do.OutcomeId
	WHERE DocumentId in (SELECT DocumentId from @Docs)

	SELECT DocumentId, StudyId, OutcomeId FROM Document_Outcome
	where DocumentId in (SELECT DocumentId from @Docs)

RETURN 0
