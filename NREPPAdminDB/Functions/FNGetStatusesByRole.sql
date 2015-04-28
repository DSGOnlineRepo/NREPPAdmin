CREATE FUNCTION [dbo].[FNGetStatusesByRole]
(
	@RoleName nvarchar(256)
)
RETURNS @returntable TABLE
(
	statusId INT
)
AS
BEGIN
	
	IF @RoleName = 'Review Coordinator' BEGIN -- Use the english versions?
		INSERT @returntable
		SELECT Id from InterventionStatus where Id > 1
	END ELSE IF @RoleName = 'Principal Investigator'
		INSERT INTO @returntable 
			SELECT Id from InterventionStatus WHERE Id < 90
	ELSE IF @RoleName = 'DSG PRM'
		INSERT INTO @returntable
			SELECT Id from InterventionStatus WHERE Id = 5
	RETURN
END
