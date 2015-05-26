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
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList, LitReviewDone
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE 
		 (@Title IS NULL OR Title LIKE '%' + @Title + '%')
		 AND (@FullDescription IS NULL OR FullDescription LIKE '%' + @FullDescription + '%')
		 AND (@UpdatedDate IS NULL OR (Convert(date,UpdateDate) = Convert(date, @UpdatedDate)))
		 AND ((@Submitter IS NULL OR u.Firstname LIKE '%' + @Submitter + '%') OR (@Submitter IS NULL OR u.Lastname LIKE '%' + @Submitter + '%'))
		 AND  i.Id in (SELECT statusId from @AvailStatus)
		)

		SELECT InterventionId, Title, FullDescription, Submitter, SubmitterId, StatusName,
		StatusId, PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList, LitReviewDone
		FROM InterventionsList
		WHERE rowNum BETWEEN @intStartRow AND @intEndRow
	END
	ELSE BEGIN
		SELECT i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList,
		LitReviewDone
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id and i.Id in (SELECT statusId from @AvailStatus)
	END
		
RETURN 0

GO


alter FUNCTION [dbo].[FNHavePermission]
(
	@inPermission VARCHAR(30),
	@InterventionId INT = NULL,
	@UserName nvarchar(128)
)
RETURNS BIT
AS
BEGIN
	DECLARE @InterventionStatus INT
	DECLARE @CanDo BIT
	DECLARE @UserRole nvarchar(256)

	DECLARE @ThePermission INT
	DECLARE @UserId NVARCHAR(128) = (SELECT ID FROM ASPNETUSERS WHERE USERNAME = @UserName)

	SELECT @ThePermission = Id FROM
	Permissions WHERE PermissionName = @inPermission

	IF @InterventionId IS NOT NULL BEGIN
		SELECT @InterventionStatus = Status from Interventions
		WHERE Id = @InterventionId

		-- Do Intervention-Based Rules First

		DECLARE @IntervRole NVARCHAR(128)
		
		SELECT @IntervRole = WkRoleId from Inter_User_Roles
		WHERE InterventionId = @InterventionId AND UserId = @UserId

		IF @IntervRole IS NOT NULL BEGIN
			SELECT @CanDo = Allowed from Role_Permissions
			WHERE RoleID = @IntervRole AND StatusID = @InterventionStatus AND PermissionID = @ThePermission

			-- If the permission isn't explicitly defined, fall back on roles
			IF @CanDo IS NULL BEGIN

				SELECT @CanDo = Allowed from Role_Permissions
				WHERE RoleID = @IntervRole AND PermissionID = @ThePermission ANd StatusID = null
			END
		END
		ELSE BEGIN
			SELECT @UserRole = RoleId from AspNetRoles r
			INNER JOIN AspNetUserRoles ur ON ur.RoleId = r.Id
			INNER JOIN AspNetUsers u on u.Id = ur.UserId
			WHERE u.Id = @UserId

			SELECT @CanDo = Allowed from Role_Permissions
			WHERE RoleID = @UserRole AND PermissionID = @ThePermission
		END
	END
	ELSE BEGIN
		SELECT @UserRole = RoleId from AspNetRoles r
		INNER JOIN AspNetUserRoles ur ON ur.RoleId = r.Id
		INNER JOIN AspNetUsers u on u.Id = ur.UserId
		WHERE u.Id = @UserId

		SELECT @CanDo = Allowed from Role_Permissions
		WHERE RoleID = @UserRole AND PermissionID = @ThePermission
	END

	RETURN @CanDo
END
