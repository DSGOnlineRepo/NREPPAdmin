CREATE PROCEDURE [dbo].[SPGetRolesOnInv]
	@InvId INT
AS
	SELECT WkRoleId, UserId, Name FROM Inter_User_Roles ir
	INNER JOIN AspNetRoles r ON ir.WkRoleId = r.Id
	WHERE InterventionId = @InvId
RETURN 0
