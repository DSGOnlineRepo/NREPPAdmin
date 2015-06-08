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
		WHERE iru.WkRoleId in (Select roles.Id from AspNetRoles roles where roles.Name in (Select RoleName from @RoleGroups where GroupName = 'dsg'))
		and s.StatusName != 'Pending Submission'	
	END ELSE 
	BEGIN IF @filterGroup = 'admin'
		INSERT INTO @returntable SELECT Id from Interventions i 
		INNER JOIN Inter_User_Roles iru ON i.Id = iru.InterventionId
	END
	
	return
END
GO

Alter Table Reviewers add ContractEndDate datetime 
GO

Alter Table Reviewers add DocumentId int 
GO

CREATE PROCEDURE [dbo].[SPUpdateReviewer]
	@Id NVARCHAR(128) , 
	@FirstName NVARCHAR(250) , 
    @LastName NVARCHAR(250) , 
    @Degree VARCHAR(35) , 
    @ReviewerType VARCHAR(35) ,        
    @HomeAddressLine1 VARCHAR(100) , 
    @HomeAddressLine2 VARCHAR(100) , 
	@HomeCity VARCHAR(30) ,
    @HomeState CHAR(2) , 
    @HomeZip VARCHAR(11) , 
	@PhoneNumber VARCHAR(15) , 
    @FaxNumber VARCHAR(15) , 
    @Email VARCHAR(100) , 
    @Employer VARCHAR(40) , 
    @Department VARCHAR(30) , 
    @WorkAddressLine1 VARCHAR(100) , 
	@WorkAddressLine2 VARCHAR(100) , 
    @WorkCity VARCHAR(30) ,
	@WorkState CHAR(2) ,
    @WorkZip VARCHAR(11) , 
    @WorkPhoneNumber VARCHAR(15) , 
    @WorkFaxNumber VARCHAR(15) , 
    @WorkEmail NCHAR(100) , 
    @ExperienceSummary VARCHAR(MAX) , 
	@Active BIT,
	@ModifiedBy nvarchar(128),
	@ModifiedOn datetime,
	@ContractEndDate datetime
AS

UPDATE [dbo].[Reviewers] SET
            [FirstName] = @FirstName
           ,[LastName] = @LastName
           ,[Degree] = @Degree
           ,[ReviewerType] = @ReviewerType
           ,[HomeAddressLine1] = @HomeAddressLine1
           ,[HomeAddressLine2] = @HomeAddressLine2
           ,[HomeCity] = @HomeCity
           ,[HomeState] = @HomeState
           ,[HomeZip] = HomeZip
           ,[PhoneNumber] = @PhoneNumber
           ,[FaxNumber] = @FaxNumber
           ,[Email] = @Email
           ,[Employer] = @Employer
           ,[Department] = @Department
           ,[WorkAddressLine1] = @WorkAddressLine1
           ,[WorkAddressLine2] = @WorkAddressLine2
           ,[WorkCity] = @WorkCity
           ,[WorkState] = @WorkState
           ,[WorkZip] = @WorkZip
           ,[WorkPhoneNumber] = @WorkPhoneNumber
           ,[WorkFaxNumber] = @WorkFaxNumber
           ,[WorkEmail] = @WorkEmail
           ,[ExperienceSummary] = @ExperienceSummary
           ,[Active] = @Active
		   ,[ModifiedBy] = @ModifiedBy
		   ,[ModifiedOn] = @ModifiedOn
		   ,[ContractEndDate] = @ContractEndDate
		WHERE ID = @Id
RETURN 0	

GO


ALTER PROCEDURE [dbo].[SPAddReviewer]
	@UserId NVARCHAR(128) , 
	@FirstName NVARCHAR(250) , 
    @LastName NVARCHAR(250) , 
    @Degree VARCHAR(35) , 
    @ReviewerType VARCHAR(35) ,        
    @HomeAddressLine1 VARCHAR(100) , 
    @HomeAddressLine2 VARCHAR(100) , 
	@HomeCity VARCHAR(30) ,
    @HomeState CHAR(2) , 
    @HomeZip VARCHAR(11) , 
	@PhoneNumber VARCHAR(15) , 
    @FaxNumber VARCHAR(15) , 
    @Email VARCHAR(100) , 
    @Employer VARCHAR(40) , 
    @Department VARCHAR(30) , 
    @WorkAddressLine1 VARCHAR(100) , 
	@WorkAddressLine2 VARCHAR(100) , 
    @WorkCity VARCHAR(30) ,
	@WorkState CHAR(2) ,
    @WorkZip VARCHAR(11) , 
    @WorkPhoneNumber VARCHAR(15) , 
    @WorkFaxNumber VARCHAR(15) , 
    @WorkEmail NCHAR(100) , 
    @ExperienceSummary VARCHAR(MAX) , 
	@Active BIT,
	@CreatedBy nvarchar(128),
	@ModifiedBy nvarchar(128),
	@CreatedOn datetime,
	@ModifiedOn datetime,
	@ContractEndDate datetime
AS

INSERT INTO [dbo].[Reviewers]
           ([Id]
           ,[UserId]
           ,[FirstName]
           ,[LastName]
           ,[Degree]
           ,[ReviewerType]
           ,[HomeAddressLine1]
           ,[HomeAddressLine2]
           ,[HomeCity]
           ,[HomeState]
           ,[HomeZip]
           ,[PhoneNumber]
           ,[FaxNumber]
           ,[Email]
           ,[Employer]
           ,[Department]
           ,[WorkAddressLine1]
           ,[WorkAddressLine2]
           ,[WorkCity]
           ,[WorkState]
           ,[WorkZip]
           ,[WorkPhoneNumber]
           ,[WorkFaxNumber]
           ,[WorkEmail]
           ,[ExperienceSummary]
           ,[Active]
		   ,[CreatedBy]
		   ,[ModifiedBy]
		   ,[CreatedOn]
		   ,[ModifiedOn]
		   ,[ContractEndDate])
     VALUES
           (NewId()
           ,@UserId
           ,@FirstName
           ,@LastName
           ,@Degree
           ,@ReviewerType
           ,@HomeAddressLine1
           ,@HomeAddressLine2
           ,@HomeCity
           ,@HomeState
           ,@HomeZip
           ,@PhoneNumber
           ,@FaxNumber
           ,@Email
           ,@Employer
           ,@Department
           ,@WorkAddressLine1
           ,@WorkAddressLine2
           ,@WorkCity
           ,@WorkState
           ,@WorkZip
           ,@WorkPhoneNumber
           ,@WorkFaxNumber
           ,@WorkEmail
           ,@ExperienceSummary
           ,@Active
		   ,@CreatedBy
		   ,@ModifiedBy
		   ,@CreatedOn
		   ,@ModifiedOn
		   ,@ContractEndDate)

RETURN 0

GO


ALTER PROCEDURE [dbo].[SPGetReviewers]
	@Id NVARCHAR(128) = null,
	@FirstName NVARCHAR(250) = null,
	@LastName NVARCHAR(250) = null,
	@Employer VARCHAR(35) = null,
	@Department VARCHAR(35) = null,
	@ReviewerType VARCHAR(35) = null,
	@Degree VARCHAR(35) = null,
	@Page int = 1,
	@PageLength int = 10
AS

DECLARE @intStartRow int;
DECLARE @intEndRow int;

SET @intStartRow = @Page;
SET @intEndRow = (@Page + @PageLength) - 1;    

WITH reviewerList AS
    (
	SELECT  ROW_NUMBER() Over(ORDER BY r.FirstName) as rowNum, COUNT(r.Id) OVER() AS searchTotal, r.Id, a.UserName, a.Id as UserId,  r.FirstName, r.LastName, Degree, ReviewerType, r.HomeAddressLine1, r.HomeAddressLine2, r.HomeCity, r.HomeState, r.HomeZip, r.PhoneNumber, r.FaxNumber,
	r.Email, r.Employer, r.Department, r.WorkAddressLine1, r.WorkAddressLine2, r.WorkCity, r.WorkState, r.WorkZip, r.WorkPhoneNumber, r.WorkFaxNumber, r.ExperienceSummary,
	r.WorkEmail, Active,  r.ContractEndDate, d.FileName, d.Description as ContractDescription, d.Id as DocId from Reviewers r
	LEFT JOIN AspNetUsers a ON r.UserId = a.Id
	left join document d on d.Id = r.DocumentId
	WHERE (@Id IS NULL OR r.Id LIKE '%' + @Id + '%')
		AND (@FirstName IS NULL OR r.FirstName LIKE '%' + @FirstName + '%')
		AND (@LastName IS NULL OR r.LastName LIKE '%' + @LastName + '%')
		AND (@Employer IS NULL OR r.Employer LIKE '%' + @Employer + '%')
		AND (@Department IS NULL OR r.Department LIKE '%' + @Department + '%')
		AND (@ReviewerType IS NULL OR ReviewerType LIKE '%' + @ReviewerType + '%')
		AND (@Degree IS NULL OR Degree LIKE '%' + @Degree + '%')
		)

	SELECT Id, UserName, UserId, FirstName, LastName, Degree, ReviewerType, HomeAddressLine1, HomeAddressLine2, HomeCity, HomeState, HomeZip, PhoneNumber, FaxNumber,
	Email, Employer, Department, WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber, WorkFaxNumber, ExperienceSummary,
	WorkEmail, Active, ContractEndDate, FileName, ContractDescription, DocId, searchTotal FROM reviewerList
WHERE rowNum BETWEEN @intStartRow AND @intEndRow
	
RETURN 0

GO


ALTER PROCEDURE [dbo].[SPGetUsers]
	@UserName NVARCHAR(256),
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Email VARCHAR(256),
	@RoleName VARCHAR(256),
	@Page INT,
	@PageLength INT
AS

DECLARE @intStartRow int;
DECLARE @intEndRow int;

SET @intStartRow = (@Page -1) * @PageLength + 1;
SET @intEndRow = @Page * @PageLength;    

WITH usersList AS
    (
	SELECT  ROW_NUMBER() OVER(ORDER BY FirstName) AS rowNum, COUNT(AspNetUsers.Id) OVER() AS searchTotal, 
		AspNetUsers.Id, UserName, FirstName, LastName, HomeAddressLine1, HomeAddressLine2, 
		HomeCity, HomeState, HomeZip, PhoneNumber, FaxNumber, Email, Employer, Department,
		WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber,
		WorkFaxNumber, ExperienceSummary, WorkEmail, Case when (LockoutEnabled = 1) then (Case when (LockoutEndDateUtc <= SYSDATETIMEOFFSET()) then 0 else 1 end) else 0 end as IsUserLocked,
		aspnetroles.Name as RoleName
	FROM AspNetUsers
	inner join AspNetUserRoles on AspNetUserRoles.UserId = AspNetUsers.Id
	inner join aspnetroles on aspnetroles.Id = AspNetUserRoles.RoleId
	WHERE (@UserName IS NULL OR UserName LIKE '%' + @UserName + '%')
		AND (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
		AND (@LastName IS NULL OR LastName LIKE '%' + @LastName + '%')
		AND (@Email IS NULL OR Email LIKE '%' + @Email + '%')
		AND (@RoleName IS NULL OR aspnetroles.Name LIKE '%' + @RoleName + '%')
		)

	SELECT Id, UserName, FirstName, LastName, HomeAddressLine1, HomeAddressLine2, 
		HomeCity, HomeState, HomeZip, PhoneNumber, FaxNumber, Email, Employer, Department, 
		WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber, 
		WorkFaxNumber, WorkEmail, IsUserLocked, searchTotal, RoleName
	FROM usersList
	WHERE rowNum BETWEEN @intStartRow AND @intEndRow

RETURN 0

GO

