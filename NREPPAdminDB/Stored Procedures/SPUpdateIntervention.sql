﻿CREATE PROCEDURE [dbo].[SPUpdateIntervention]
	@IntervId int = -1,
	@title varchar(50) = '',
	@fulldescription ntext,
	@submitterId nvarchar(128),
	@updateDate DateTime,
	@publishDate DateTime = NULL,
	@programType INT = 0,
	@Acronym VARCHAR(20) = NULL,
	@status int,
	@IsLitSearch bit = 0,
	@PreScreenAnswers INT = 0,
	@UserPreScreenAnswer INT = 0,
	@ScreeningNotes VARCHAR(MAX) = NULL,
	@Output INT OUTPUT
AS SET NOCOUNT ON

	BEGIN TRANSACTION

	-- Do the insert portion first
	IF @IntervId = -1 BEGIN

		INSERT INTO Interventions (Title, FullDescription, PublishDate, UpdateDate, SubmitterId, Status, ProgramType, Acronym, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes) VALUES
			(@title, @fulldescription, @publishDate, @updateDate, @submitterId, @status, @programType, @Acronym, @PreScreenAnswers, @UserPreScreenAnswer, @ScreeningNotes)


		if @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		SELECT @Output = @@IDENTITY

		DECLARE @nUserId VARCHAR(128)
		DECLARE @nSubmitterRole VARCHAR(128)

		--SELECT @nUserId = Id from AspNetUsers WHERE UserName = @submitterId
		SELECT @nSubmitterRole = Id from AspNetRoles where Name = 'Principal Investigator'

		INSERT INTO Inter_User_Roles (InterventionId, WkRoleId, UserId) VALUES (@Output, @nSubmitterRole, @submitterId)

		if @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -11
		END

		--INSERT INTO Inter_User_Roles (UserId, 

		
	END
	ELSE BEGIN
		-- Now do update portion
		UPDATE Interventions
			SET title = @title,
			FullDescription = @fulldescription,
			PublishDate = @publishDate,
			UpdateDate = @updateDate,
			SubmitterId = @submitterId,
			Status = @status,
			ProgramType = @programType,
			Acronym = @Acronym,
			PreScreenAnswers = @PreScreenAnswers,
			UserPreScreenAnswer = @UserPreScreenAnswer,
			ScreeningNotes = @ScreeningNotes
		WHERE Id = @IntervId

		SET @Output = @IntervId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END
	END

	COMMIT TRANSACTION
	
RETURN 0
