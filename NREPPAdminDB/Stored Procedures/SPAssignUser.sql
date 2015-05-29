CREATE PROCEDURE [dbo].[SPAssignUser]
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
