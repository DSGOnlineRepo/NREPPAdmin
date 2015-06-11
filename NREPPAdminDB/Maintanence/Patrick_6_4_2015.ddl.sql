CREATE TABLE [dbo].[Interv_Users_ReviewStatus]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [InterventionID] INT NOT NULL,
	[UserID] INT NOT NULL,
	[ReviewerStatus] VARCHAR(75) NOT NULL
)

GO

CREATE PROCEDURE SPGetReviewerByInterv
	@InterventionId INT

AS SET NOCOUNT ON

SELECT UserId, FirstName, LastName, ReviewerStatus from Interv_Users_ReviewStatus iurs
INNER JOIN AspNetUsers u on iurs.UserID = u.Id
WHERE InterventionID = @InterventionId

GO

CREATE PROCEDURE SPAssignReviewer
	@InterventionID INT,
	@UserId VARCHAR(128),
	@ReviewerStatus VARCHAR(75)

AS SET NOCOUNT ON

	BEGIN TRANSACTION

	SELECT @UserId = UserId from Reviewers
	WHERE Id = @UserId

	IF EXISTS(SELECT TOP 1 Id from Interv_Users_ReviewStatus where @InterventionID = InterventionID AND UserID = @UserId)
	BEGIN
		UPDATE Interv_Users_ReviewStatus
		SET ReviewerStatus = @ReviewerStatus
		WHERE InterventionID = @InterventionID AND UserId = @UserId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END
	END
	ELSE BEGIN
		INSERT INTO Interv_Users_ReviewStatus (InterventionID, UserID, ReviewerStatus) VALUES (@InterventionID, @UserId, @ReviewerStatus)

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END
	END

	COMMIT TRANSACTION

RETURN 0