ALTER PROCEDURE [dbo].[GetReviewerByInterv]
	@InterventionId INT

AS SET NOCOUNT ON

SELECT u.Id, u.FirstName, u.LastName, ReviewerStatus from Interv_Users_ReviewStatus iurs
INNER JOIN Reviewers u on iurs.UserID = u.Id
WHERE InterventionID = @InterventionId
GO

ALTER FUNCTION [dbo].[FNGetAvailIntervs]
(
	@UserId NVARCHAR(128),
	@filterGroup NVARCHAR(256)
)
RETURNS @returntable TABLE
(
	Id int
)
AS
BEGIN

	DECLARE @UserRoleName NVARCHAR(256)	
	DECLARE @UserRoleGroup NVARCHAR(256)	
	DECLARE @RoleGroups table (RoleName NVarchar(256), GroupName NVARCHAR(128))

	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Principal Investigator', 'user')
	-- These users have Mathematica Access
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Mathematica Assigner', 'mathematica')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Mathematica Review Coordinator', 'mathematica')
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Mathematica PRM', 'mathematica') -- This maybe should be in Personal?

	-- These users have generic Access
	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Review Coordinator', 'dsg')	
    INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('PRM', 'dsg')

	INSERT INTO @RoleGroups (RoleName, GroupName) VALUES ('Administrator', 'admin')


	-- Get the role so you can do some generic logic

	SELECT @UserRoleName = r.Name FROM AspNetUserRoles ur
	INNER JOIN AspNetRoles r ON Id = RoleId
	WHERE UserId = @UserId
	
	SELECT @UserRoleGroup = r.GroupName FROM @RoleGroups r
	WHERE r.RoleName = @UserRoleName
	
	--INSERT INTO @returntable Select Id from Interventions

	If @filterGroup IS NULL OR @filterGroup = ''
	BEGIN
		set @filterGroup = @UserRoleGroup
	END 
		
	IF @filterGroup = 'user' BEGIN			
		INSERT INTO @returntable SELECT Id from Interventions i 
		INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
		WHERE UserId = @UserId
	END
	ELSE IF  @filterGroup = 'mathematica' 
	BEGIN
		INSERT INTO @returntable SELECT i.Id from Interventions i 
		INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
		INNER JOIN InterventionStatus s ON s.Id = i.Status
		WHERE iru.WkRoleId in (Select roles.Id from AspNetRoles roles where roles.Name in (Select RoleName from @RoleGroups where GroupName = 'mathematica'))
		and s.StatusName != 'Pending Submission'
	END
	ELSE IF @filterGroup = 'dsg'
	BEGIN
		INSERT INTO @returntable SELECT i.Id from Interventions i 
		INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
		INNER JOIN InterventionStatus s ON s.Id = i.Status
		WHERE s.StatusName != 'Pending Submission'	
	END ELSE 
	BEGIN IF @filterGroup = 'admin'
		INSERT INTO @returntable SELECT Id from Interventions i 
		INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
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
@filterGroup nvarchar(256)=NULL
AS
SET NOCOUNT ON

	DECLARE @InterventionIdList table (statusId INT)
	DECLARE @intStartRow int;
	DECLARE @intEndRow int;
	DECLARE @UserId NVARCHAR(128)

	SET @intStartRow = @Page;
	SET @intEndRow = (@Page + @PageLength) - 1; 

	select @UserId = Id from AspNetUsers WHERE UserName = @UserName
	print @UserId
	
	INSERT INTO @InterventionIdList SELECT Id from FNGetAvailIntervs(@UserId, @filterGroup)	

	-- TODO: break this function into two different functions

	IF @Id IS NULL BEGIN
	WITH InterventionsList AS
    (
		SELECT ROW_NUMBER() Over(ORDER BY FirstName) as rowNum, COUNT(i.Id) OVER() AS searchTotal, i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList, LitReviewDone,
		[PrimaryName],
		[PrimaryOrg],
		[PrimaryTitle],
		[PrimaryAddressLine1],
		[PrimaryAddressLine2],
		[PrimaryCity],
		[PrimaryState],
		[PrimaryZip],
		[PrimaryPhoneNumber],
		[PrimaryFaxNumber],
		[PrimaryEmail],
		[SecondaryName],
		[SecondaryOrg],
		[SecondaryTitle],
		[SecondaryAddressLine1],
		[SecondaryAddressLine2],
		[SecondaryCity],
		[SecondaryState],
		[SecondaryZip],
		[SecondaryPhoneNumber],
		[SecondaryFaxNumber],
		[SecondaryEmail]
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE 
		 (@Title IS NULL OR Title LIKE '%' + @Title + '%')
		 AND (@FullDescription IS NULL OR FullDescription LIKE '%' + @FullDescription + '%')
		 AND (@UpdatedDate IS NULL OR (Convert(date,UpdateDate) = Convert(date, @UpdatedDate)))
		 AND ((@Submitter IS NULL OR u.Firstname LIKE '%' + @Submitter + '%') OR (@Submitter IS NULL OR u.Lastname LIKE '%' + @Submitter + '%'))
		 AND  i.Id in (SELECT statusId from @InterventionIdList)
		)

		SELECT InterventionId, Title, FullDescription, Submitter, SubmitterId, StatusName,
		StatusId, PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList, LitReviewDone,
		[PrimaryName],
		[PrimaryOrg],
		[PrimaryTitle],
		[PrimaryAddressLine1],
		[PrimaryAddressLine2],
		[PrimaryCity],
		[PrimaryState],
		[PrimaryZip],
		[PrimaryPhoneNumber],
		[PrimaryFaxNumber],
		[PrimaryEmail],
		[SecondaryName],
		[SecondaryOrg],
		[SecondaryTitle],
		[SecondaryAddressLine1],
		[SecondaryAddressLine2],
		[SecondaryCity],
		[SecondaryState],
		[SecondaryZip],
		[SecondaryPhoneNumber],
		[SecondaryFaxNumber],
		[SecondaryEmail],
		searchTotal
		FROM InterventionsList
		WHERE rowNum BETWEEN @intStartRow AND @intEndRow
	END
	ELSE BEGIN
		SELECT i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList, LitReviewDone,
		[PrimaryName],
		[PrimaryOrg],
		[PrimaryTitle],
		[PrimaryAddressLine1],
		[PrimaryAddressLine2],
		[PrimaryCity],
		[PrimaryState],
		[PrimaryZip],
		[PrimaryPhoneNumber],
		[PrimaryFaxNumber],
		[PrimaryEmail],
		[SecondaryName],
		[SecondaryOrg],
		[SecondaryTitle],
		[SecondaryAddressLine1],
		[SecondaryAddressLine2],
		[SecondaryCity],
		[SecondaryState],
		[SecondaryZip],
		[SecondaryPhoneNumber],
		[SecondaryFaxNumber],
		[SecondaryEmail],
		1 as searchTotal
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id and i.Id in (SELECT statusId from @InterventionIdList)
	END
		
RETURN 0
GO