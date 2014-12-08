CREATE PROCEDURE [dbo].[SPGetUser]
	@userName varchar(30) = null -- Done so that we can get a larger user list
AS

	DECLARE @RoleID INT

	SELECT @RoleID = RoleID from Users

	SELECT Id, Username, Firstname, Lastname, RoleID  FROM Users
	WHERE Username = @userName

	SELECT Id as RoleId, RoleName, ViewInterventions, ViewAllUsers, ViewPendingInterventions, CreateUser,
		CreateIntervention, CreateReview, AccesPIComments, EmailEditor, EmailPI, EmailQC, EmailSAMHSA, CanBeContacted,
		ChangeProgStatus, ChangeAccess, AssignStaff, DeleteProgram from Roles
	WHERE Id = @RoleID
RETURN 0
