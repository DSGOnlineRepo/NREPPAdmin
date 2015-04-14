/*
	Make sure to flesh this out as you get more functionality to implement
*/

CREATE PROCEDURE [dbo].[SPChangeInterventionStatus]
	@IntervId int,
	@User int, -- Person performing the operation
	@DestStatus int,
	@DestUser int = NULL
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
				DECLARE @NewUser INT
				DECLARE @OldUser INT

				SELECT @NewUser = CAST(Value AS INT) FROM AppSettings
					WHERE  SettingID = 'NextPreScreen'

				SELECT @OldUser = CAST(Value AS INT) FROM AppSettings
					WHERE  SettingID = 'CurrentPreScreen'

				INSERT Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @NewUser, 2)

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -2
				END

				UPDATE AppSettings
					SET Value = CAST(@OldUser as VARCHAR)
					WHERE SettingID = 'NextPreScreen'

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -21
				END

				UPDATE AppSettings
					SET Value = CAST(@NewUser as VARCHAR)
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
				INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @DestUser, 5)

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -4
				END
			END

			IF @DestStatus = 5 BEGIN
				INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@IntervId, @DestUser, 6)

				IF @@ERROR <> 0 BEGIN
					ROLLBACK TRANSACTION
					RETURN -5
				END
			END

			
	COMMIT TRANSACTION
RETURN 0
