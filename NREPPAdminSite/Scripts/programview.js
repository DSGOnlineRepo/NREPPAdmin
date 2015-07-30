function getParameterByName(name) {
    var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
    return match && decodeURIComponent(match[1].replace(/\+/g, ' '));
}

$(".programMask").click(function () {
    var sum = 0;
    $(".programMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#programType").val(sum);
});

$("#btnToggleEdit").click(function () {
    $(".someform").removeClass("hidden");
    $(".description").addClass("hidden");
});

$("#DocButton").click(function () {
    $(".numOfDocs").addClass("hidden");
    $(".upload-div").removeClass("hidden");
});

$("#AddStudy").click(function () {
    $(".AddStudy").addClass("hidden");
    $(".Add-div").removeClass("hidden");
});

$("#btnCancelAddStudy").click(function () {
    $(".AddStudy").removeClass("hidden");
    $(".Add-div").addClass("hidden");
});

$("#AddOutcome").click(function () {
    $(".AddOutcome").addClass("hidden");
    $(".AddO-div").removeClass("hidden");
});

/*$(".preScreenMask").click(function () {
    var sum = 0;
    $(".preScreenMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#preScreenAns").val(sum);
});*/

/*$(".userPreScreenMask").click(function () {
    var sum = 0;
    $(".userPreScreenMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#userPreScreenAns").val(sum);
    
        if (sum === 10 || sum === 18 || sum === 12 || sum === 20) {
            $(".prescreenWarnDiv").addClass("hidden");
        } else {
            $(".prescreenWarnDiv").removeClass("hidden");
        }
});*/

function setEdit(index) {
    var rcnameId = "txtRCName_" + index.toString();
    var referenceId = "txtRef_" + index.toString();
    var pubyear = "txtPubYear_" + index.toString();
    var cancelName = "cancel_" + index.toString();
    var dirtyId = "#" + "isdirty_" + index.toString();
    var chkId = "chkAddToReview_" + index.toString();

    $("#" + rcnameId).removeClass("hidden");
    $("#" + referenceId).removeClass("hidden");
    $("#" + pubyear).removeClass("hidden");
    $("button[name*=" + cancelName + "]").removeClass("hidden");
    $("button[name*=" + cancelName + "]").siblings("input[type=submit]").removeClass("hidden");
    $("button[name*=btnEdit").addClass("hidden");
    $("#" + chkId).prop('disabled', false);

    $("button[name*=btnEdit]").each(function () {
        $(this).prop("disabled", true);
    });

    $(dirtyId).val("true");
}

function clearEdit(index) {
    var rcnameId = "txtRCName_" + index.toString();
    var referenceId = "txtRef_" + index.toString();
    var pubyear = "txtPubYear_" + index.toString();
    var cancelName = "cancel_" + index.toString();
    var dirtyId = "#" + "isdirty_" + index.toString();
    var chkId = "chkAddToReview_" + index.toString();

    $("#" + rcnameId).addClass("hidden");
    $("#" + referenceId).addClass("hidden");
    $("#" + pubyear).addClass("hidden");
    $("button[name*=" + cancelName + "]").addClass("hidden");
    $("button[name*=" + cancelName + "]").siblings("input[type=submit]").addClass("hidden");
    $("button[name*=btnEdit").removeClass("hidden");
    $("#" + chkId).prop('disabled', true);
    

    $("button[name*=btnEdit]").each(function () {
        $(this).prop("disabled", false);
    });

    $(dirtyId).val("false");

}


$(".preScreenMask").click(function () {
    var sum = 0;
    $(".preScreenMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#preScreenAns").val(sum);
});
