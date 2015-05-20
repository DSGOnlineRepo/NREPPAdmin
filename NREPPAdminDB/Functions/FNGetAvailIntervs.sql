CREATE FUNCTION [dbo].[FNGetAvailIntervs]
(
	@UserId int,
	@IsRelevant bit = 0
)
RETURNS @returntable TABLE
(
	Id int
)
AS
BEGIN

	DECLARE @RoleID NVARCHAR(256)
	DECLARE @RoleName NVARCHAR(256)
	
	-- This maybe needs to be a table, but I think it works okay this way
	DECLARE @RoleGroups table (RoleName NVarchar(256), GroupName NVARCHAR(128))

	-- These users can only see their own stuff

	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Principal Investigator', 'Personal')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Reviewer', 'Personal')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('PRM', 'Personal')

	-- These users have Mathematica Access
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Mathematica Assigner', 'Mathematica')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Mathematica Review Coordinator', 'Mathematica')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Mathematica PRM', 'Mathematica') -- This maybe should be in Personal?

	-- These users have generic Access
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('DSG Assigner', 'Generic')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('DSG Review Coordinator', 'Generic')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Administrator', 'Generic')


	-- Get the role so you can do some generic logic

	SELECT @RoleID = RoleId, @RoleName = r.Name FROM AspNetUserRoles ur
	INNER JOIN AspNetRoles r ON Id = RoleId
	WHERE UserId = @UserId

	
	--INSERT INTO @returntable Select Id from Interventions

	If @IsRelevant <> 0 BEGIN
	-- Need to work in the fact that there is some limitation

		INSERT INTO @returntable SELECT Id from Interventions i 
		INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
		WHERE UserId = @UserId
	END ELSE BEGIN

		DECLARE @SomeVal NVARCHAR(128)

		SELECT @SomeVal = GroupName from @RoleGroups
		WHERE RoleName = @RoleName

		IF @SomeVal = 'Personal' BEGIN
			
			INSERT INTO @returntable SELECT Id from Interventions i 
			INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
			WHERE UserId = @UserId

		END
		ELSE BEGIN

			IF  @SomeVal = 'Mathematica' BEGIN
				INSERT INTO @returntable SELECT Id from Interventions i 
				INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
				WHERE UserId = @UserId
				UNION
				SELECT Id from Interventions i 
				INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
				WHERE Status BETWEEN 10 AND 19 -- This will change if the mathematica definition changes
			END
			ELSE BEGIN
				INSERT INTO @returntable SELECT Id from Interventions i 
				INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
			END
		END

	END

	return
END
