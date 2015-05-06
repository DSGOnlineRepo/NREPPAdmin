CREATE PROCEDURE [dbo].[SPGetReviewers]
	@Id NVARCHAR(128),
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Employer VARCHAR(35),
	@Department VARCHAR(35),
	@ReviewerType VARCHAR(35),
	@Degree VARCHAR(35),
	@Page int,
	@PageLength int
AS

DECLARE @intStartRow int;
DECLARE @intEndRow int;

SET @intStartRow = @Page;
SET @intEndRow = (@Page + @PageLength) - 1;    

WITH reviewerList AS
    (
	SELECT  ROW_NUMBER() Over(ORDER BY FirstName) as rowNum, COUNT(Id) OVER() AS searchTotal, Id, UserId, FirstName, LastName, Degree, ReviewerType, HomeAddressLine1, HomeAddressLine2, HomeCity, HomeState, HomeZip, PhoneNumber, FaxNumber,
	Email, Employer, Department, WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber, WorkFaxNumber, ExperienceSummary,
	WorkEmail from Reviewers
	WHERE (@Id IS NULL OR Id LIKE '%' + @Id + '%')
		AND (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
		AND (@LastName IS NULL OR LastName LIKE '%' + @LastName + '%')
		AND (@Employer IS NULL OR Employer LIKE '%' + @Employer + '%')
		AND (@Department IS NULL OR Department LIKE '%' + @Department + '%')
		AND (@ReviewerType IS NULL OR ReviewerType LIKE '%' + @ReviewerType + '%')
		AND (@Degree IS NULL OR Degree LIKE '%' + @Degree + '%')
		)

	SELECT Id, UserId, FirstName, LastName, Degree, ReviewerType, HomeAddressLine1, HomeAddressLine2, HomeCity, HomeState, HomeZip, PhoneNumber, FaxNumber,
	Email, Employer, Department, WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber, WorkFaxNumber, ExperienceSummary,
	WorkEmail, searchTotal FROM reviewerList
WHERE rowNum BETWEEN @intStartRow AND @intEndRow

	--SELECT InterventionId from Inter_User_Roles
	--	WHERE UserId = @UserId AND WkRoleId = 7 -- Reviewer Role

	--SELECT ReviewerId, AreaOfExpertise from Reviewer_Expertise
	--	WHERE ReviewerId IN (SELECT Id from Reviewers
	--WHERE (@UserId IS NULL OR UserId = @UserId) AND
	--(@ReviewerTypeId IS NULL OR ReviewerType = @ReviewerTypeId)
	--AND (@NameField IS NULL OR FirstName like '%' + @NameField + '%' or LastName like '%' + @NameField + '%'))

RETURN 0