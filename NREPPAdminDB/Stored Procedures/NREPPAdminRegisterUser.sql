CREATE PROCEDURE [dbo].[NREPPAdminRegisterUser]
	@userName varchar(30),
	@fname varchar(50),
	@lname varchar(50),
	@hash varchar(100),
	@salt varchar(100)
AS
	SELECT @userName, @fname
RETURN 0
