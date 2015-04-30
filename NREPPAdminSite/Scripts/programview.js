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

$("#AddOutcome").click(function () {
    $(".AddOutcome").addClass("hidden");
    $(".AddO-div").removeClass("hidden");
});

$(".preScreenMask").click(function () {
    var sum = 0;
    $(".preScreenMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#preScreenAns").val(sum);
});

$(".userPreScreenMask").click(function () {
    var sum = 0;
    $(".userPreScreenMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#userPreScreenAns").val(sum);
});