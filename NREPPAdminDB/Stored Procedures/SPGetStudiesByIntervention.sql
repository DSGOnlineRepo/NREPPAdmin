﻿CREATE PROCEDURE [dbo].[SPGetStudiesByIntervention]
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
		[BaselineEquiv], 
		[UseMultivariate], 
		[LongestFollowup], 
		[SAMHSARelated], 
		[TargetPop],
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
