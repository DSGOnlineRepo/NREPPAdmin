$('#testButton').click(function () {
    populateForm(0);
});

function populateForm(FormId)
{
    var ject = currentStudy[FormId];

    $("input[name*='StudyId']").val(ject.StudyId)
    $("input[name*='IsLitSearch'").prop('checked', ject.inLitSearch);
    $("input[name*='useMultivariate']").prop('checked', ject.UseMultivariate);
    $('#docDropdown').val(ject.DocumentId);
    $("select[name*='Exclusion1']").val(ject.Exclusion1);
    $("select[name*='Exclusion2']").val(ject.Exclusion2);
    $("select[name*='Exclusion3']").val(ject.Exclusion3);
    $("select[name*='StudyDesign']").val(ject.StudyDesign);
    $("textarea[name*='Notes']").text(ject.Notes);
    $("textarea[name*='BaselineEquiv']").text(ject.BaselineEquiv);
}