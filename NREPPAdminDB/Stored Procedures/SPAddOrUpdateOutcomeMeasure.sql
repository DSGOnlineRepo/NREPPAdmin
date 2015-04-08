CREATE PROCEDURE [dbo].[SPAddOrUpdateOutcomeMeasure]
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
			--PrimaryOutcome = @PrimaryOutcome,
			--Priority = @Priority,
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
			--BaselineEquiv,
			--SignificantImpact,
			GroupFavored,
			PopDescription,
			SAMHSAPop,
			--PrimaryOutcome,
			--Priority,
			DocId)
			VALUES (@OutcomeId,
			@StudyId,
			@OutcomeMeasure,
			--@BaselineEquiv,
			--@SignificantImpact,
			@GroupFavored,
			@PopDescription,
			@SAMHSAPop,
			--@PrimaryOutcome,
			--@Priority,
			@DocId)

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -3
		END
	END

	COMMIT TRANSACTION

RETURN 0
