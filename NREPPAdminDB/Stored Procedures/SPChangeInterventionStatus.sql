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

		UPDATE Interventions
			SET Owner = @DestUser,
			Status = @DestStatus
		WHERE Id = @IntervId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END
			
	COMMIT TRANSACTION
RETURN 0
