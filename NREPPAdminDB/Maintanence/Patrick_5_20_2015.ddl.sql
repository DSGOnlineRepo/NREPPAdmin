ALTER FUNCTION [dbo].[FNGetAvailIntervs]
(
	@UserId NVARCHAR(128),
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
GO

ALTER PROCEDURE [dbo].[SPGetInterventionList]
@Id INT = NULL,
@UserName nvarchar(256) = NULL,
@Title NVARCHAR(50) = NULL,
@FullDescription NVARCHAR(100) = NULL,
@UpdatedDate DATETIME = NULL,
@Submitter NVARCHAR(100) = NULL,
@Page INT=NULL,
@PageLength INT=NULL,
@IsRelevant BIT = 1
AS
SET NOCOUNT ON

	DECLARE @AvailStatus table (statusId INT)
	DECLARE @intStartRow int;
	DECLARE @intEndRow int;
	DECLARE @UserId NVARCHAR(128)

	SET @intStartRow = @Page;
	SET @intEndRow = (@Page + @PageLength) - 1; 

	select @UserId = Id from AspNetUsers WHERE UserName = @UserName
	INSERT INTO @AvailStatus SELECT Id from FNGetAvailIntervs(@UserId, @IsRelevant)

	IF @Id IS NULL BEGIN
	WITH InterventionsList AS
    (
		SELECT ROW_NUMBER() Over(ORDER BY FirstName) as rowNum, i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE 
		 (@Title IS NULL OR Title LIKE '%' + @Title + '%')
		 AND (@FullDescription IS NULL OR FullDescription LIKE '%' + @FullDescription + '%')
		 AND (@UpdatedDate IS NULL OR (Convert(date,UpdateDate) = Convert(date, @UpdatedDate)))
		 AND ((@Submitter IS NULL OR u.Firstname LIKE '%' + @Submitter + '%') OR (@Submitter IS NULL OR u.Lastname LIKE '%' + @Submitter + '%'))
		 AND  s.Id in (SELECT statusId from @AvailStatus)
		)

		SELECT InterventionId, Title, FullDescription, Submitter, SubmitterId, StatusName,
		StatusId, PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes
		FROM InterventionsList
		WHERE rowNum BETWEEN @intStartRow AND @intEndRow
	END
	ELSE BEGIN
		SELECT i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id and i.Id in (SELECT statusId from @AvailStatus)
	END
		
RETURN 0
GO
