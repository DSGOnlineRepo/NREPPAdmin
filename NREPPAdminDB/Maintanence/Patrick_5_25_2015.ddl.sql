DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Review'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer')

INSERT INTO Permissions (Id, PermissionName) VALUES (6, 'CanLitReview')

INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (6, @ROLEID4, 4, CAST(1 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleID, StatusID, Allowed) values (4, @ROLEID4, NULL, cast(1 as Bit))

GO

ALTER PROCEDURE [dbo].[SPUpdateIntervention]
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
	@HaveMaterials BIT = 0,
	@MaterialsList VARCHAR(MAX) = NULL,
	@LitReviewDone BIT = 0,
	@Output INT OUTPUT
AS SET NOCOUNT ON

	BEGIN TRANSACTION

	-- Do the insert portion first
	IF @IntervId = -1 BEGIN

		INSERT INTO Interventions (Title, FullDescription, PublishDate, UpdateDate, SubmitterId, Status, ProgramType, Acronym, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes,
		HaveMaterials, MaterialsList) VALUES
			(@title, @fulldescription, @publishDate, @updateDate, @submitterId, @status, @programType, @Acronym, @PreScreenAnswers, @UserPreScreenAnswer, @ScreeningNotes, @HaveMaterials, @MaterialsList)


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
			ScreeningNotes = @ScreeningNotes,
			HaveMaterials = @HaveMaterials,
			MaterialsList = @MaterialsList,
			LitReviewDone = @LitReviewDone
		WHERE Id = @IntervId

		SET @Output = @IntervId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END
	END

	COMMIT TRANSACTION
	
RETURN 0
GO

/********************************************************************************************
			EVENING CHANGES

*********************************************************************************************/

/*
	Make sure to flesh this out as you get more functionality to implement
*/

ALTER PROCEDURE [dbo].[SPChangeInterventionStatus]
	@IntervId int,
	@User nvarchar(128) = NULL, -- Person performing the operation
	@DestStatus int,
	@DestUser nvarchar(128) = NULL
AS SET NOCOUNT ON

	BEGIN TRANSACTION

		DECLARE @CurrStatus INT

		SELECT @CurrStatus = Status from Interventions
		WHERE Id = @IntervId


			UPDATE Interventions
				SET Status = @DestStatus
			WHERE Id = @IntervId

			IF @@ERROR <> 0 BEGIN
				ROLLBACK TRANSACTION
				RETURN -1
			END

			IF @DestStatus = 2 BEGIN
				DECLARE @NewUser nvarchar(128)
				DECLARE @OldUser NVARCHAR(128)

				SELECT @NewUser = Value FROM AppSettings
					WHERE  SettingID = 'NextPreScreen'

				SELECT @OldUser = Value FROM AppSettings
					WHERE  SettingID = 'CurrentPreScreen'

					-- Need to add a new role for this.
				INSERT Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @NewUser, (select id from AspNetRoles where NAME = 'PreScreener')) 

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -2
				END

				UPDATE AppSettings
					SET Value = @OldUser
					WHERE SettingID = 'NextPreScreen'

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -21
				END

				UPDATE AppSettings
					SET Value = @NewUser
					WHERE SettingID = 'CurrentPreScreen'

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -22
				END


			END

			IF @DestStatus = 3 BEGIN
				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -3
				END
			END

			IF @DestStatus = 4 BEGIN

				DECLARE @LitReviewDone BIT

				SELECT @LitReviewDone = LitReviewDone from Interventions
				WHERE Id = @IntervId

				IF @LitReviewDone <> 1 BEGIN
					ROLLBACK TRANSACTION
					RETURN -41
				END

				INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @DestUser, (select id from AspNetRoles where NAME = 'Review Coordinator'))

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -4
				END
			END

			IF @DestStatus = 5 BEGIN
				INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @DestUser, (select id from AspNetRoles where NAME = 'DSG PRM'))

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -5
				END
			END

			
	COMMIT TRANSACTION
RETURN 0

GO

DECLARE @ROLEID1 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Data Entry'),
		@ROLEID2 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Assigner'),
		@ROLEID3 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Principal Investigator'), 
		@ROLEID4 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Lit Review'), 
		@ROLEID5 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Review Coordinator'), 
		@ROLEID6 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'DSG PRM'), 
		@ROLEID7 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Mathematica Assigner'), 
		@ROLEID8 NVARCHAR(128) = (SELECT ID FROM ASPNETROLES WHERE NAME = 'Reviewer')

INSERT INTO Permissions (Id, PermissionName) VALUES (7, 'AssignLitReview')

INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (7, @ROLEID2, 3, CAST(1 AS BIT))
INSERT INTO Role_Permissions (PermissionID, RoleId, StatusId, Allowed) values (7, @ROLEID2, 3, CAST(1 AS BIT))

GO

UPDATE RoutingTable SET
DestDescription = 'Assign to RC'
WHERE CurrentStatus = 3 AND DestStatus = 4

UPDATE RoutingTable SET
DestDescription = 'Failed Minimum Requirements'
WHERE CurrentStatus = 2 AND DestStatus = 92

UPDATE RoutingTable SET
DestDescription = 'Return to Assigner'
WHERE CurrentStatus = 4 AND DestStatus = 3

UPDATE RoutingTable SET
DestDescription = 'Send to PRM'
WHERE CurrentStatus = 4 AND DestStatus = 5


GO