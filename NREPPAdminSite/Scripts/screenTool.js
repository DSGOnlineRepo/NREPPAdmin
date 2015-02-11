
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

    $(dirtyId).val("false");

}
