GO

ALTER TABLE OutcomeMeasure
ADD InstrumentSource VARCHAR(MAX) NULL,
EffectSource VARCHAR(MAX) NULL,
GeneralDescription VARCHAR(MAX) NULL,
AssessmentPd INT NULL,
LongestFollowup INT null,
FullSample INT NULL

GO

ALTER PROCEDURE [dbo].[SPGetOutcomesByInterventionId]
	@InterventionId INT
AS

	DECLARE  @Docs TABLE (DocumentId int)

	INSERT INTO @Docs (DocumentId) select Id from Document where InterventionId = @InterventionId

	SELECT om.Id as [OutcomeMeasureId], OutcomeMeasure, EffectSource, InstrumentSource, GeneralDescription, AssessmentPd, LongestFollowup, FullSample,
		GroupFavored, PopDescription, SAMHSAPop, SAMHSAOutcome, EffectReport, StudyId, DocId as DocumentId, o.Id as OutcomeId,
		RecommendReview, TaxonomyOutcome from OutcomeMeasure om
	INNER JOIN Outcome o ON o.Id = om.OutcomeId
	WHERE InterventionId = @InterventionId

	SELECT Id, OutcomeName from Outcome 
	WHERE InterventionId = @InterventionId

	SELECT DocumentId, StudyId, OutcomeId FROM Document_Outcome
	where DocumentId in (SELECT DocumentId from @Docs)

RETURN 0

GO

ALTER PROCEDURE [dbo].[SPAddOrUpdateOutcomeMeasure]
	@OutcomeId INT = -1,
	@OutcomeName VARCHAR(30) = NULL,
	@OutcomeMeasureId INT = -1,
	@InterventionId INT = 0,
	@StudyId INT,
	@OutcomeMeasure VARCHAR(50) = '',
	@SAMHSAOutcome INT = 0,
	@GroupFavored BIT = 0,
	@PopDescription VARCHAR(50) = '',
	@SAMHSAPop INT = 0,
	@EffectReport BIT = 0,
	@Priority INT = 0,
	@TaxonomyOutcome INT = 0,
	@RecommendReview BIT = 0,
	@EffectSource VARCHAR(MAX) = NULL,
	@InstrumentSource VARCHAR(MAX) = NULL,
	@GeneralDescription VARCHAR(MAX) = NULL,
	@AssessmentPd INT = 0,
	@LongestFollowup INT = 0,
	@FullSample INT = 0,
	@DocId INT
AS

	BEGIN TRANSACTION

	-- Add the outcome if it doesn't already exist

	IF @OutcomeId > 0 BEGIN
		SELECT @OutcomeName = OutcomeName FROM Outcome
		WHERE Id = @OutcomeId
	END
	ELSE BEGIN
		INSERT INTO Outcome (OutcomeName, InterventionId) VALUES (@OutcomeName, @InterventionId)

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		SET @OutcomeId = @@IDENTITY
	END

	-- Add or Update the Outcome Measure

	IF @OutcomeMeasureId > 0 BEGIN
		UPDATE OutcomeMeasure
		SET OutcomeId = @OutcomeId,
			StudyId = @StudyId,
			OutcomeMeasure = @OutcomeMeasure,
			--BaselineEquiv = @BaselineEquiv,
			--SignificantImpact = @SignificantImpact,
			GroupFavored = @GroupFavored,
			PopDescription = @PopDescription,
			SAMHSAPop = @SAMHSAPop,
			SAMHSAOutcome = @SAMHSAOutcome,
			RecommendReview = @RecommendReview,
			TaxonomyOutcome = @TaxonomyOutcome,
			EffectReport = @EffectReport,
			EffectSource = @EffectSource,
			GeneralDescription = @GeneralDescription,
			InstrumentSource = @InstrumentSource,
			AssessmentPd = @AssessmentPd,
			@LongestFollowup = @LongestFollowup,
			@FullSample = @FullSample,
			DocId = @DocId
		WHERE Id = @OutcomeMeasureId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END
	END
	ELSE BEGIN

		INSERT INTO OutcomeMeasure (
			OutcomeId,
			StudyId,
			OutcomeMeasure,
			GroupFavored,
			PopDescription,
			SAMHSAPop,
			RecommendReview,
			TaxonomyOutcome,
			SAMHSAOutcome,
			EffectReport,
			DocId,
			EffectSource,
			GeneralDescription,
			InstrumentSource,
			AssessmentPd, LongestFollowup, FullSample)
			VALUES (@OutcomeId,
			@StudyId,
			@OutcomeMeasure,
			@GroupFavored,
			@PopDescription,
			@SAMHSAPop,
			@RecommendReview,
			@TaxonomyOutcome,
			@SAMHSAOutcome,
			@EffectReport,
			@DocId,
			@EffectSource,
			@GeneralDescription,
			@InstrumentSource,
			@AssessmentPd, @LongestFollowup, @FullSample)

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -3
		END
	END

	COMMIT TRANSACTION

RETURN 0
