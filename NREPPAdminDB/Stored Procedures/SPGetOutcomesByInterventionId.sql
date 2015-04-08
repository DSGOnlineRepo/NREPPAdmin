CREATE PROCEDURE [dbo].[SPGetOutcomesByInterventionId]
	@InterventionId INT
AS

	DECLARE  @Docs TABLE (DocumentId int)

	INSERT INTO @Docs (DocumentId) select Id from Document where InterventionId = @InterventionId

	SELECT om.Id as [OutcomeMeasureId], OutcomeMeasure,
		GroupFavored, PopDescription, SAMHSAPop, SAMHSAOutcome, EffectReport, StudyId, DocId as DocumentId, o.Id as OutcomeId,
		RecommendReview, TaxonomyOutcome from OutcomeMeasure om
	INNER JOIN Outcome o ON o.Id = om.OutcomeId
	WHERE InterventionId = @InterventionId

	SELECT Id, OutcomeName from Outcome 
	WHERE InterventionId = @InterventionId

	SELECT DocumentId, StudyId, OutcomeId FROM Document_Outcome
	where DocumentId in (SELECT DocumentId from @Docs)

RETURN 0
