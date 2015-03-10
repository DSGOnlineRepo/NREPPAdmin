CREATE PROCEDURE [dbo].[SPAddReviewer]
	@ReviewerId INT = -1, 
	@UserId INT = NULL,
	@Degree int = NULL,
	@ReviewerType INT = NULL,
	@IsActive BIT = 0,
	@FirstName VARCHAR(30) = NULL,
	@LastName VARCHAR(35) = NULL,
	@StreetAddress VARCHAR(100) = NULL,
	@City VARCHAR(30) = null,
	@State CHAR(20) = NULL,
	@ZIP VARCHAR(11) = NULL,
	@HomeEmail VARCHAR(40) = NULL,
	@WorkStreetAddress VARCHAR(100) = NULL,
	@WorkCity VARCHAR(30) = null,
	@WorkState CHAR(20) = NULL,
	@WorkZIP VARCHAR(11) = NULL,
	@WorkEmail VARCHAR(40) = NULL,
	@Employer VARCHAR(40) = NULL,
	@Department VARCHAR(30) = NULL,
	@Experience VARCHAR(MAX),
	@OutId INT OUTPUT
AS set nocount on
	
	BEGIN TRANSACTION
	
	IF @ReviewerId < 1 BEGIN
		INSERT INTO Reviewers (UserId, 
			Degree, 
			ReviewerType, 
			IsActive, 
			FirstName, 
			LastName, 
			StreetAddress, 
			--Phone, 
			City,
			State, 
			ZIP, 
			--FaxNumber, 
			HomeEmail, 
			Employer, 
			Department, 
			WorkStreetAddress, 
			WorkCity,
			WorkState,
			WorkZip, 
			--WorkPhone, 
			--WorkFax, 
			WorkEmail, 
			ExperienceSummary) VALUES
			(@UserId,
			@Degree,
			@ReviewerType,
			@IsActive,
			@FirstName,
			@LastName,
			@StreetAddress,
			@City,
			@State,
			@ZIP,
			@HomeEmail,
			@Employer,
			@Department,
			@WorkStreetAddress,
			@WorkCity,
			@WorkState,
			@WorkZIP,
			@WorkEmail,
			@Experience)

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		SET @OutId = @@IDENTITY
	END
	ELSE BEGIN
		UPDATE Reviewers
		SET UserId = @UserId,
			Degree = @Degree,
			ReviewerType = @ReviewerType,
			IsActive = @IsActive,
			FirstName = @FirstName,
			LastName = @LastName,
			StreetAddress = @StreetAddress,
			City = @City,
			State = @State,
			ZIP = @ZIP,
			HomeEmail = @HomeEmail,
			WorkStreetAddress = @WorkStreetAddress,
			WorkCity = @WorkCity,
			WorkState = @WorkState,
			WorkZIP = @WorkZIP,
			WorkEmail = @WorkEmail,
			Employer = @Employer,
			Department = @Department,
			ExperienceSummary = @Experience
		WHERE Id = @ReviewerId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END

		SET @OutId = @ReviewerId
	END

	COMMIT TRANSACTION
RETURN 0
