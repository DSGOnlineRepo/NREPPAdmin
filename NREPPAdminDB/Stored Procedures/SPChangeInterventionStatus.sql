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
