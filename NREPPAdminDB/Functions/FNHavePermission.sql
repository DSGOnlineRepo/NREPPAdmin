CREATE FUNCTION [dbo].[FNHavePermission]
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
	DECLARE @UserRole INT

	DECLARE @ThePermission INT

	SELECT @ThePermission = Id FROM
	Permissions WHERE PermissionName = @inPermission

	IF @InterventionId IS NOT NULL BEGIN
		SELECT @InterventionStatus = Status from Interventions
		WHERE Id = @InterventionId

		-- Do Intervention-Based Rules First

		DECLARE @IntervRole NVARCHAR(128)
		DECLARE @UserId NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = @UserName)
		
		SELECT @IntervRole = WkRoleId from Inter_User_Roles
		WHERE InterventionId = @InterventionId AND UserId = @UserId

		IF @IntervRole IS NOT NULL BEGIN
			SELECT @CanDo = Allowed from Role_Permissions
			WHERE RoleID = @IntervRole AND StatusID = @InterventionStatus AND PermissionID = @ThePermission

			-- If the permission isn't explicitly defined, fall back on roles
			IF @CanDo IS NULL BEGIN

				SELECT @CanDo = Allowed from Role_Permissions
				WHERE RoleID = @IntervRole AND PermissionID = @ThePermission
			END
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
