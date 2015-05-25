GO
ALTER TABLE Interventions
ADD LitReviewDone BIT NOT NULL
CONSTRAINT LitReviewDone DEFAULT 0
GO

CREATE PROCEDURE [dbo].[SPGetRolesOnInv]
	@InvId INT
AS
	SELECT WkRoleId, UserId, Name FROM Inter_User_Roles
	WHERE InterventionId = @InvId
RETURN 0
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
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList, LitReviewDone
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

ALTER PROCEDURE [dbo].[SPGetDestinations]
	@IntervId int
AS SET NOCOUNT ON
	DECLARE @CurrStatus INT
	DECLARE @destTable TABLE (DestStatus INT, UserRole nvarchar(128), DestDescription VARCHAR(500))

	SELECT @CurrStatus = Status from Interventions WHERE Id = @IntervId

	INSERT INTO @destTable SELECT DestStatus, DestUserRole, DestDescription from RoutingTable
		WHERE CurrentStatus = @CurrStatus


	SELECT DestStatus as StatusId, u.Id as UserId, r.Name as RoleName, u.Firstname + ' ' + u.Lastname as [UserName], StatusName, DestDescription from @destTable dt
	INNER JOIN AspNetUserRoles ur on ur.RoleID = dt.UserRole
	INNER JOIN AspNetUsers u on u.Id = ur.UserId
	INNER JOIN AspNetRoles r on r.Id = dt.UserRole
	INNER JOIN InterventionStatus s on dt.DestStatus = s.Id
	UNION
	SELECT DestStatus as StatusId, null as UserId, null as RoleName, StatusName as [UserName], StatusName, DestDescription from @destTable dt
	inner join InterventionStatus s ON dt.DestStatus = s.Id
	where dt.UserRole IS NULL

RETURN 0

GO