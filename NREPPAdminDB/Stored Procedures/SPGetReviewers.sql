CREATE PROCEDURE [dbo].[SPGetReviewers]
	@ReviewerTypeId int = NULL,
	@UserId nvarchar(128) = NULL,
	@NameField VARCHAR(35) = NULL
AS
	SELECT Id, Degree, ReviewerType, FirstName, LastName, HomeAddressLine1, HomeAddressLine2, HomeCity, HomeState, HomeZip, PhoneNumber, HomeFaxNumber
	Employer, Email, Department, WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber, WorkFaxNumber, ExperienceSummary
	WorkEmail from AspNetUsers
	WHERE (@UserId IS NULL OR Id = @UserId) AND
	(@ReviewerTypeId IS NULL OR ReviewerType = @ReviewerTypeId)
	AND (@NameField IS NULL OR FirstName like '%' + @NameField + '%' or LastName like '%' + @NameField + '%')

	SELECT InterventionId from Inter_User_Roles
		WHERE UserId = @UserId AND WkRoleId = 7 -- Reviewer Role

	SELECT ReviewerId, AreaOfExpertise from Reviewer_Expertise
		WHERE ReviewerId IN (SELECT Id from Reviewers
	WHERE (@UserId IS NULL OR UserId = @UserId) AND
	(@ReviewerTypeId IS NULL OR ReviewerType = @ReviewerTypeId)
	AND (@NameField IS NULL OR FirstName like '%' + @NameField + '%' or LastName like '%' + @NameField + '%'))

RETURN 0
