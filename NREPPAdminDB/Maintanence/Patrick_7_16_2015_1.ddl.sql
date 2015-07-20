GO

ALTER TABLE Interventions
ADD IsLive BIT NOT NULL DEFAULT 0

GO

USE [NREPPAdmin]
GO
/****** Object:  StoredProcedure [dbo].[SPGetInterventionList]    Script Date: 7/20/2015 9:51:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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
@filterGroup nvarchar(256) = 'user'
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
		[SecondaryEmail],
		[IsLive]
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
		[IsLive],
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
		[IsLive],
		1 as searchTotal
		from Interventions i 
		inner join AspNetUsers u ON i.SubmitterId = u.Id
		inner join InterventionStatus s on i.Status = s.Id
		WHERE i.Id = @Id and i.Id in (SELECT statusId from @InterventionIdList)
	END
		
RETURN 0

GO