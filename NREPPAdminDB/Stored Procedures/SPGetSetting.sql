CREATE PROCEDURE [dbo].[SPGetSetting]
	@InSetting VARCHAR(30)
AS
	SELECT Value from AppSettings
		WHERE SettingID = @InSetting
RETURN 0
