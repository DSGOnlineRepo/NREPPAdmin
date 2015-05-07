GO
CREATE PROCEDURE [dbo].[SPGetUsers]
	@UserName NVARCHAR(256),
	@FirstName NVARCHAR(250),
	@LastName NVARCHAR(250),
	@Email VARCHAR(256),
	@LockoutEnabled BIT = NULL,	
	@Page INT,
	@PageLength INT
AS

DECLARE @intStartRow int;
DECLARE @intEndRow int;

SET @intStartRow = (@Page -1) * @PageLength + 1;
SET @intEndRow = @Page * @PageLength;    

WITH usersList AS
    (
	SELECT  ROW_NUMBER() OVER(ORDER BY FirstName) AS rowNum, COUNT(Id) OVER() AS searchTotal, 
		Id, UserName,FirstName, LastName, Degree, ReviewerType, HomeAddressLine1, HomeAddressLine2, 
		HomeCity, HomeState, HomeZip, PhoneNumber,HomeFaxNumber, Email, Employer, Department,
		WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber,
		WorkFaxNumber, ExperienceSummary, WorkEmail, LockoutEnabled 
	FROM AspNetUsers
	WHERE (@UserName IS NULL OR UserName LIKE '%' + @UserName + '%')
		AND (@FirstName IS NULL OR FirstName LIKE '%' + @FirstName + '%')
		AND (@LastName IS NULL OR LastName LIKE '%' + @LastName + '%')
		AND (@Email IS NULL OR Email LIKE '%' + @Email + '%')
		AND (@LockoutEnabled IS NULL OR LockoutEnabled = @LockoutEnabled)	
		)

	SELECT Id, UserName, FirstName, LastName, Degree, ReviewerType, HomeAddressLine1, HomeAddressLine2, 
		HomeCity, HomeState, HomeZip, PhoneNumber, HomeFaxNumber, Email, Employer, Department, 
		WorkAddressLine1, WorkAddressLine2, WorkCity, WorkState, WorkZip, WorkPhoneNumber, 
		WorkFaxNumber, ExperienceSummary,WorkEmail,LockoutEnabled, searchTotal 
	FROM usersList
	WHERE rowNum BETWEEN @intStartRow AND @intEndRow

RETURN 0