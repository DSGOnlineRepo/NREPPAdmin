ALTER TABLE Interventions
ADD MaterialsList VARCHAR(Max),
HaveMaterials BIT
CONSTRAINT HaveMaterials DEFAULT 0
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
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList
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
		StatusId, PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList
		FROM InterventionsList
		WHERE rowNum BETWEEN @intStartRow AND @intEndRow
	END
	ELSE BEGIN
		SELECT i.Id as InterventionId, Title, FullDescription, u.Firstname + ' ' + u.Lastname as [Submitter], i.SubmitterId as [SubmitterId], StatusName,
		s.Id as [StatusId], PublishDate, UpdateDate, ProgramType, Acronym, Owner, FromListSearch, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes, HaveMaterials, MaterialsList
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id and i.Id in (SELECT statusId from @AvailStatus)
	END
		
RETURN 0
GO

ALTER PROCEDURE [dbo].[SPUpdateIntervention]
	@IntervId int = -1,
	@title varchar(50) = '',
	@fulldescription ntext,
	@submitterId nvarchar(128),
	@updateDate DateTime,
	@publishDate DateTime = NULL,
	@programType INT = 0,
	@Acronym VARCHAR(20) = NULL,
	@status int,
	@IsLitSearch bit = 0,
	@PreScreenAnswers INT = 0,
	@UserPreScreenAnswer INT = 0,
	@ScreeningNotes VARCHAR(MAX) = NULL,
	@HaveMaterials BIT = 0,
	@MaterialsList VARCHAR(MAX) = NULL,
	@Output INT OUTPUT
AS SET NOCOUNT ON

	BEGIN TRANSACTION

	-- Do the insert portion first
	IF @IntervId = -1 BEGIN

		INSERT INTO Interventions (Title, FullDescription, PublishDate, UpdateDate, SubmitterId, Status, ProgramType, Acronym, PreScreenAnswers, UserPreScreenAnswer, ScreeningNotes,
		HaveMaterials, MaterialsList) VALUES
			(@title, @fulldescription, @publishDate, @updateDate, @submitterId, @status, @programType, @Acronym, @PreScreenAnswers, @UserPreScreenAnswer, @ScreeningNotes, @HaveMaterials, @MaterialsList)


		if @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -1
		END

		SELECT @Output = @@IDENTITY

		DECLARE @nUserId VARCHAR(128)
		DECLARE @nSubmitterRole VARCHAR(128)

		--SELECT @nUserId = Id from AspNetUsers WHERE UserName = @submitterId
		SELECT @nSubmitterRole = Id from AspNetRoles where Name = 'Principal Investigator'

		INSERT INTO Inter_User_Roles (InterventionId, WkRoleId, UserId) VALUES (@Output, @nSubmitterRole, @submitterId)

		if @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -11
		END

		--INSERT INTO Inter_User_Roles (UserId, 

		
	END
	ELSE BEGIN
		-- Now do update portion
		UPDATE Interventions
			SET title = @title,
			FullDescription = @fulldescription,
			PublishDate = @publishDate,
			UpdateDate = @updateDate,
			SubmitterId = @submitterId,
			Status = @status,
			ProgramType = @programType,
			Acronym = @Acronym,
			PreScreenAnswers = @PreScreenAnswers,
			UserPreScreenAnswer = @UserPreScreenAnswer,
			ScreeningNotes = @ScreeningNotes,
			HaveMaterials = @HaveMaterials,
			MaterialsList = @MaterialsList
		WHERE Id = @IntervId

		SET @Output = @IntervId

		IF @@ERROR <> 0 BEGIN
			ROLLBACK TRANSACTION
			RETURN -2
		END
	END

	COMMIT TRANSACTION
	
RETURN 0
