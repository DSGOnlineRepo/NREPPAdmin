CREATE PROCEDURE [dbo].[SPGetDestinations]
	@IntervId int
AS SET NOCOUNT ON
	DECLARE @CurrStatus INT
	DECLARE @destTable TABLE (DestStatus INT, UserRole nvarchar(128))

	SELECT @CurrStatus = Status from Interventions WHERE Id = @IntervId

	INSERT INTO @destTable SELECT DestStatus, DestUserRole from RoutingTable
		WHERE CurrentStatus = @CurrStatus


	SELECT DestStatus as StatusId, u.Id as UserId, r.Name as RoleName, u.Firstname + ' ' + u.Lastname as [UserName], StatusName from @destTable dt
	INNER JOIN AspNetUserRoles ur on ur.RoleID = dt.UserRole
	INNER JOIN AspNetUsers u on u.Id = ur.UserId
	INNER JOIN AspNetRoles r on r.Id = dt.UserRole
	INNER JOIN InterventionStatus s on dt.DestStatus = s.Id
	UNION
	SELECT DestStatus as StatusId, null as UserId, null as RoleName, StatusName as [UserName], StatusName from @destTable dt
	inner join InterventionStatus s ON dt.DestStatus = s.Id
	where dt.UserRole IS NULL

RETURN 0