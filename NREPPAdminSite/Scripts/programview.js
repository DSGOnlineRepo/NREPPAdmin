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