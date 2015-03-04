CREATE PROCEDURE [dbo].[SPGetDestinations]
	@IntervId int
AS SET NOCOUNT ON
	DECLARE @CurrStatus INT
	DECLARE @destTable TABLE (DestStatus INT, UserRole INT)

	SELECT @CurrStatus = Status from Interventions WHERE Id = @IntervId

	INSERT INTO @destTable SELECT DestStatus, DestUserRole from RoutingTable
		WHERE CurrentStatus = @CurrStatus


	SELECT DestStatus as StatusId, u.Id as UserId, RoleName, u.Firstname + ' ' + u.Lastname as [UserName] from @destTable dt
	INNER JOIN Users u on u.RoleID = dt.UserRole
	INNER JOIN Roles r on r.Id = dt.UserRole

RETURN 0
