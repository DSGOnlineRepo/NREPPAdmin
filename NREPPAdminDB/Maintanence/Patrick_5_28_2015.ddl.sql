-- Nothing to See here

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
				INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @DestUser, (select id from AspNetRoles where NAME = 'Assigner'))
				
				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -3
				END
			END

			IF @DestStatus = 4 BEGIN

				/*DECLARE @LitReviewDone BIT

				SELECT @LitReviewDone = LitReviewDone from Interventions
				WHERE Id = @IntervId

				IF @LitReviewDone <> 1 BEGIN
					ROLLBACK TRANSACTION
					RETURN -41
				END*/

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

ALTER PROCEDURE [dbo].[SPAssignUser]
	@InvId int = 0,
	@UserId varchar(128),
	@RoleId varchar(128)
AS
	BEGIN TRANSACTION

	IF NOT EXISTS(Select top(1) UserId from Inter_User_Roles WHERE InterventionId = @InvId AND UserId = @UserId AND @RoleId = @RoleId) BEGIN
	
	INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@InvId, @UserId, @RoleId)

	IF @@ERROR <> 0 BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END

	END

	COMMIT TRANSACTION
RETURN 0

GO


