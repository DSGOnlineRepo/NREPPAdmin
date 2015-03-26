
function populateStudyForm(FormId)
{
    var ject = currentStudy[FormId];

    $("input[name*='StudyId']").val(ject.StudyId)
    $("input[name*='IsLitSearch'").prop('checked', ject.inLitSearch);
    $("input[name*='useMultivariate']").prop('checked', ject.UseMultivariate);
    $("input[name*='recommendReview']").prop('checked', ject.RecommendReview);
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
    $("select[name*='MacroOutcome']").val(ject.OutcomeId);
}

$(document).ready(function () {
    $(".chosen-select").chosen();
});

$("#AddStudy").click(function () {
    $(".AddStudy").addClass("hidden");
    $(".Add-div").removeClass("hidden");
});

$("#AddOutcome").click(function () {
    $(".AddOutcome").addClass("hidden");
    $(".AddO-div").removeClass("hidden");
});

function setEdit(index)
{
    var rcnameId = "txtRCName_" + index.toString();
    var referenceId = "txtRef_" + index.toString();
    var pubyear = "txtPubYear_" + index.toString();
    var cancelName = "cancel_" + index.toString();
    var dirtyId = "#" + "isdirty_" + index.toString();

    $("#" + rcnameId).removeClass("hidden");
    $("#" + referenceId).removeClass("hidden");
    $("#" + pubyear).removeClass("hidden");
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
    var pubyear = "txtPubYear_" + index.toString();
    var cancelName = "cancel_" + index.toString();
    var dirtyId = "#" + "isdirty_" + index.toString();

    $("#" + rcnameId).addClass("hidden");
    $("#" + referenceId).addClass("hidden");
    $("#" + pubyear).addClass("hidden");
    $("button[name*=" + cancelName + "]").addClass("hidden");
    $("button[name*=" + cancelName + "]").siblings("input[type=submit]").addClass("hidden");

    $("button[name*=btnEdit]").each(function () {
        $(this).prop("disabled", false);
    });

    $(dirtyId).val("false");

}

function GetDocumentInfo(inDocId, incrtl)
{
    var reference = $(incrtl).siblings("textarea[name*='Reference']");
    var pubYear = $(incrtl).siblings("input[name*='PubYear']");
    var docIdThing = $(incrtl).siblings("input[name*='RCDocumentId']");
    var rcName = $(incrtl).siblings("input[name*='RCDocumentName']");

    for(var i = 0; i < docsList.length; i++)
    {
        if (docsList[i].Id == inDocId)
        {
            $(reference).text(docsList[i].Reference);
            $(pubYear).val(docsList[i].inYear);
            $(docIdThing).val(docsList[i].rcDocId);
            $(rcName).val(docsList[i].rcName);
            break;
        }
            
    }
}
