CREATE PROCEDURE [dbo].[SPAdminRegisterUser]
	@userName varchar(30),
	@fname varchar(50) = NULL,
	@lname varchar(50) = NULL,
	@hash varchar(100),
	@salt varchar(100),
	@RoleId int
AS
	BEGIN TRANSACTION
		INSERT INTO Users (Username, Firstname, Lastname, hash, salt, RoleID)
			VALUES (@userName, @fname, @lname, @hash, @salt, @RoleId)
		
		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END
	COMMIT TRANSACTION
RETURN 0
