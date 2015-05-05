CREATE PROCEDURE [dbo].[SPGetRecentSubPd]
AS
	SELECT top 1 StartDate, EndDate from SubmissionPds
	ORDER BY StartDate DESC
RETURN 0
