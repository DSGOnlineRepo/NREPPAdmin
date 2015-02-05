CREATE PROCEDURE [dbo].[SPAddOrUpdateStudyRecord]
	/*@param1 int = 0,
	@param2 int*/
	@Id INT,
	@StudyId INT,
    @Reference VARCHAR(100), 
    @InLitSearch BIT = 0, 
    @Exclusion1 INT = 0, 
    @Exclusion2 INT = 0, 
    @Exclusion3 INT = 0, 
    @StudyDesign INT = 0, 
    --@GroupSize] NCHAR(10) NULL, 
    @BaselineEquiv VARCHAR(MAX), 
    @UseMultivariate BIT = 0, 
    --[LongestFollowup] NCHAR(10) NULL, 
    @SAMSHARelated INT, 
    --@TargetPop NCHAR(10), 
    --@ListOfOutcomes NCHAR(10), 
    @AuthorQueryNeeded BIT = 0, 
    @RecommendReview BIT = 0, 
    @Notes VARCHAR(MAX) = NULL, 
    @DocumentId INT,
	@IDOut INT OUTPUT
AS SET NOCOUNT ON	
	
	BEGIN TRANSACTION
		IF @Id = -1 BEGIN
			INSERT INTO Studies ([StudyId],
		[Reference], 
		[FromLitSearch], 
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
		[AuthorQueryNeeded], 
		[RecommendReview], 
		[Notes], 
		[DocumentId]) VALUES (@StudyId,
    @Reference, 
    @InLitSearch, 
    @Exclusion1, 
    @Exclusion2, 
    @Exclusion3, 
    @StudyDesign, 
		null, 
		@BaselineEquiv, 
    @UseMultivariate, 
    null,
    @SAMSHARelated, 
    null,  
    @AuthorQueryNeeded, 
    @RecommendReview, 
    @Notes, 
    @DocumentId)


		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		SET @IDOut = @@IDENTITY
	END
	ELSE BEGIN

		UPDATE Studies
		SET [StudyId] = @StudyId,
			[Reference] = @Reference, 
			[InLitSearch] = @InLitSearch, 
			[Exclusion1] = @Exclusion1, 
			[Exclusion2] = @Exclusion2, 
			[Exclusion3] = @Exclusion3, 
			[StudyDesign] = @StudyDesign, 
			[GroupSize] = null, 
			[BaselineEquiv] = @BaselineEquiv, 
			[UseMultivariate] = @UseMultivariate, 
			[LongestFollowup] = null, 
			[SAMSHARelated] = @SAMSHARelated, 
			[TargetPop] = null, 
			[ListOfOutcomes] = null, 
			[AuthorQueryNeeded] = @AuthorQueryNeeded, 
			[RecommendReview] = @RecommendReview, 
			[Notes] = @Notes,
			[DocumentId] = @DocumentId
		WHERE Id = @Id

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END

		SET @IDOut = @Id
	END
	COMMIT TRANSACTION

RETURN 0
