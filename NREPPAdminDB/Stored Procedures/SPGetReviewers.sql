CREATE PROCEDURE [dbo].[SPGetReviewers]
	@ReviewerTypeId int = NULL,
	@UserId INT = NULL,
	@NameField VARCHAR(35) = NULL
AS
	SELECT Id, UserId, Degree, ReviewerType, IsActive, FirstName, LastName, StreetAddress, City, Phone,
	State, ZIP, FaxNumber, HomeEmail, Department, WorkStreetAddress, WorkCity, WorkState, WorkZip, WorkPhone,
	WorkFax, WorkEmail, ExperienceSummary from Reviewers
	WHERE (@UserId IS NULL OR UserId = @UserId) AND
	(@ReviewerTypeId IS NULL OR ReviewerType = @ReviewerTypeId)
	AND (@NameField IS NULL OR FirstName like '%' + @NameField + '%' or LastName like '%' + @NameField + '%')

	SELECT InterventionId from Inter_User_Roles
		WHERE UserId = @UserId AND WkRoleId = 7 -- Reviewer Role

RETURN 0
