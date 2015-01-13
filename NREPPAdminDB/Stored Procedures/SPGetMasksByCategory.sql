CREATE PROCEDURE [dbo].[SPGetMasksByCategory]
	@InCategory VARCHAR(50)
AS
	SELECT MaskPower, MaskValueName FROM MaskList
	WHERE MaskCategory = @InCategory
RETURN 0
