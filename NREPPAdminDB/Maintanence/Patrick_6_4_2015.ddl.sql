CREATE PROCEDURE GetReviewerByInterv
	@InterventionId INT

AS SET NOCOUNT ON

SELECT UserId, FirstName, LastName, WkRoleId, Name, Name from Inter_User_Roles iur
INNER JOIN AspNetUsers u on u.Id = iur.UserId
INNER JOIN AspNetRoles ar on iur.WkRoleId = ar.Id
WHERE InterventionId = @InterventionId AND Name in ('Reviewer', 'Invited Reviewer', 'Declined Reviewer')

GO