CREATE PROCEDURE [dbo].[SPGetStudiesByIntervention]
	@IntervId INT
AS
	SELECT 
		[Id],
		[StudyId],
		[Reference], 
		[FromLitSearch] as InLitSearch, 
		[Exclusion1], 
		[Exclusion2], 
		[Exclusion3], 
		[StudyDesign], 
		[GroupSize], 
		[UseMultivariate], 
		[LongestFollowup], 
		[SAMHSARelated], 
		[TotalSampleSize],
		[AuthorQueryNeeded], 
		[RecommendReview], 
		[Notes], 
		[DocumentId],
		[DocOrdinal],
		[OverallAttritionAvail], 
		[DiffAttritionAvail]
	FROM Studies 
	WHERE DocumentId in (select Id from Document where InterventionId = @IntervId)
RETURN 0
