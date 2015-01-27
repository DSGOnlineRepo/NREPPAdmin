CREATE PROCEDURE [dbo].[SPGetStudiesByIntervention]
	@IntervId INT
AS
	SELECT 
		[Id] ,
		[StudyId],
		[Reference], 
		[InLitSearch], 
		[Exclusion1], 
		[Exclusion2], 
		[Exclusion3], 
		[StudyDesign], 
		[GroupSize], 
		[BaselineEquiv], 
		[UseMultivariate], 
		[LongestFollowup], 
		[SAMSHARelated], 
		[TargetPop], 
		[ListOfOutcomes], 
		[AuthorQueryNeeded], 
		[RecommendReview], 
		[Notes], 
		[DocumentId]
	FROM Studies 
	WHERE DocumentId in (select DocumentId from Document where InterventionId = @IntervId)
RETURN 0
