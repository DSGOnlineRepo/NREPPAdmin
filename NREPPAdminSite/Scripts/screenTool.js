
function populateStudyForm(FormId)
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
    $("#ActualId").val(ject.Id);
}

function populateOutcomeForm(FormId)
{
    var ject = currentOutcomes[FormId];

    //$("select[name*='']").val(ject.)
    $("input[name*='OutcomeMeasureId']").val(ject.Id);
    $("input[name*='measure']").val(ject.Measure);
    $("select[name*='studySelector']").val(ject.StudyId);
    $("textarea[name*='popDescription']").text(ject.PopDescrip);
}

$(document).ready(function () {
    $(".chosen-select").chosen();
});

function setEdit(index)
{
    var rcnameId = "txtRCName_" + index.toString();
    var referenceId = "txtRef_" + index.toString();
    var cancelName = "cancel_" + index.toString();
    var dirtyId = "#" + "isdirty_" + index.toString();

    $("#" + rcnameId).removeClass("hidden");
    $("#" + referenceId).removeClass("hidden");
    $("button[name*=" + cancelName + "]").removeClass("hidden");
    $("button[name*=" + cancelName + "]").siblings("input[type=submit]").removeClass("hidden");

    $("button[name*=btnEdit]").each(function () {
        $(this).prop("disabled", true);
    });

    $(dirtyId).val("true");
}

function clearEdit(index)
{
    var rcnameId = "txtRCName_" + index.toString();
    var referenceId = "txtRef_" + index.toString();
    var cancelName = "cancel_" + index.toString();
    var dirtyId = "#" + "isdirty_" + index.toString();

    $("#" + rcnameId).addClass("hidden");
    $("#" + referenceId).addClass("hidden");
    $("button[name*=" + cancelName + "]").addClass("hidden");
    $("button[name*=" + cancelName + "]").siblings("input[type=submit]").addClass("hidden");

    $("button[name*=btnEdit]").each(function () {
        $(this).prop("disabled", false);
    });

    $(dirtyId).val("false");

}
