CREATE FUNCTION [dbo].[FNGetStatusesByRole]
(
	@RoleID INT
)
RETURNS @returntable TABLE
(
	statusId INT
)
AS
BEGIN

	IF @RoleID = 5 BEGIN -- Use the english versions?
		INSERT @returntable
		SELECT Id from InterventionStatus where Id > 1
	END ELSE IF @RoleID = 3
		INSERT INTO @returntable 
			SELECT Id from InterventionStatus WHERE Id < 90
	ELSE IF @RoleID = 6
		INSERT INTO @returntable
			SELECT Id from InterventionStatus WHERE Id = 5
	RETURN
END
