CREATE PROCEDURE [dbo].[SPUpdateIntervention]
	@IntervId int = -1,
	@title varchar(50),
	@fulldescription ntext,
	@submitter int,
	@updateDate DateTime,
	@publishDate DateTime = NULL,
	@programType INT = 0,
	@Acronym VARCHAR(20) = NULL,
	@status int,
	@Output INT OUTPUT
AS SET NOCOUNT ON

	BEGIN TRANSACTION

	SET @Output = @IntervId

	-- Do the insert portion first
	IF @IntervId = -1 BEGIN

		INSERT INTO Interventions (Title, FullDescription, PublishDate, UpdateDate, Submitter, Status, ProgramType, Acronym) VALUES
			(@title, @fulldescription, @publishDate, @updateDate, @submitter, @status, @programType, @Acronym)

		if @@error <> 0
		BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		SELECT @Output = @@IDENTITY
	END
	ELSE BEGIN
		-- Now do update portion
		UPDATE Interventions
			SET title = @title,
			FullDescription = @fulldescription,
			PublishDate = @publishDate,
			UpdateDate = @updateDate,
			Submitter = @submitter,
			Status = @status,
			ProgramType = @programType,
			Acronym = @Acronym
		WHERE Id = @IntervId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END
	END

	COMMIT TRANSACTION
	
RETURN 0
