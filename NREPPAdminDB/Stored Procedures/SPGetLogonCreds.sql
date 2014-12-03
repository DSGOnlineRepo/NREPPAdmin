/*
	Created By: Patrick Taylor
	Created On: 12/3/2014

	Purpose: Gets the hash and salt for verifying user's login
 */

CREATE PROCEDURE [dbo].[SPGetLogonCreds]
	@userName varchar(30)
AS
	SELECT hash, salt from Users
	WHERE Username = @userName
RETURN 0
