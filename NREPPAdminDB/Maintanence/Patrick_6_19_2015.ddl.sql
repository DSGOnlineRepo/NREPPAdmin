GO
ALTER PROCEDURE [dbo].[SPGetTaxOutcomes]
	@TaxId INT = NULL
AS
	IF @TaxId IS NULL BEGIN
		SELECT Id, OutcomeName, Guidelines, NotInclude from OutcomeTaxonomy
	END
	ELSE begin
		SELECT Id, OutcomeName, Guidelines, NotInclude from OutcomeTaxonomy
		WHERE Id = @TaxId
	END
RETURN 0

GO