CREATE PROCEDURE [dbo].[SPAssignUser]
	@InvId int = 0,
	@UserId varchar(128),
	@RoleId varchar(128)
AS
	BEGIN TRANSACTION
	INSERT INTO Inter_User_Roles (InterventionId, UserId, WkRoleId) VALUES (@InvId, @UserId, @RoleId)

	IF @@ERROR <> 0 BEGIN
		ROLLBACK TRANSACTION
		RETURN -1
	END

	COMMIT TRANSACTION
RETURN 0
