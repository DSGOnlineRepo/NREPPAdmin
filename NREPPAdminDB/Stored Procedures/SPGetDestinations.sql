CREATE PROCEDURE [dbo].[SPGetDestinations]
	@IntervId int
AS SET NOCOUNT ON
	DECLARE @CurrStatus INT

	SELECT @CurrStatus = Status from Interventions


RETURN 0
