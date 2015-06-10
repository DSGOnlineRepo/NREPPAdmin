GO

ALTER FUNCTION [dbo].[FNHavePermission]
(
	@inPermission VARCHAR(30),
	@InterventionId INT = NULL,
	@UserName nvarchar(128)
)
RETURNS BIT
AS
BEGIN
	DECLARE @InterventionStatus INT
	DECLARE @CanDo BIT
	DECLARE @UserRole nvarchar(256)

	DECLARE @ThePermission INT
	DECLARE @UserId NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = @UserName)

	SELECT @ThePermission = Id FROM
	Permissions WHERE PermissionName = @inPermission

	IF @InterventionId IS NOT NULL BEGIN
		SELECT @InterventionStatus = Status from Interventions
		WHERE Id = @InterventionId

		-- Do Intervention-Based Rules First

		DECLARE @IntervRole NVARCHAR(128)
		
		SELECT @IntervRole = WkRoleId from Inter_User_Roles
		WHERE InterventionId = @InterventionId AND UserId = @UserId

		IF @IntervRole IS NOT NULL BEGIN
			SELECT @CanDo = Allowed from Role_Permissions
			WHERE RoleID = @IntervRole AND StatusID = @InterventionStatus AND PermissionID = @ThePermission

			-- If the permission isn't explicitly defined, fall back on roles
			IF @CanDo IS NULL BEGIN

				SELECT @CanDo = Allowed from Role_Permissions
				WHERE RoleID = @IntervRole AND PermissionID = @ThePermission ANd StatusID IS NULL
			END
		END
		ELSE BEGIN
			SELECT @UserRole = RoleId from AspNetRoles r
			INNER JOIN AspNetUserRoles ur ON ur.RoleId = r.Id
			INNER JOIN AspNetUsers u on u.Id = ur.UserId
			WHERE u.Id = @UserId

			SELECT @CanDo = Allowed from Role_Permissions
			WHERE RoleID = @UserRole AND PermissionID = @ThePermission
		END
	END
	ELSE BEGIN
		SELECT @UserRole = RoleId from AspNetRoles r
		INNER JOIN AspNetUserRoles ur ON ur.RoleId = r.Id
		INNER JOIN AspNetUsers u on u.Id = ur.UserId
		WHERE u.Id = @UserId

		SELECT @CanDo = Allowed from Role_Permissions
		WHERE RoleID = @UserRole AND PermissionID = @ThePermission
	END

	RETURN @CanDo
END

GO

ALTER PROCEDURE [dbo].[SPAssignReviewer]
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

	IF NOT EXISTS(SELECT TOP 1 UserId from Inter_User_Roles where @InterventionID = InterventionID AND UserID = @UserId)
	BEGIN
		
		DECLARE @RoleId VARCHAR(128)

		SELECT @RoleID = Id from AspNetRoles WHERE Name = 'Reviewer'

		INSERT INTO Inter_User_Roles (UserId, WkRoleId, InterventionId) VALUES (@UserId, @RoleId, @InterventionID)
	END

	COMMIT TRANSACTION

RETURN 0