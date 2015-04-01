CREATE PROCEDURE [dbo].[SPGetTaxOutcomes]
	@TaxId INT = NULL
AS
	IF @TaxId IS NULL BEGIN
		SELECT Id, OutcomeName, Guidelines from OutcomeTaxonomy
	END
	ELSE begin
		SELECT Id, OutcomeName, Guidelines from OutcomeTaxonomy
		WHERE Id = @TaxId
	END
RETURN 0
