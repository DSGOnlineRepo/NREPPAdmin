$(".programMask").click(function () {
    var sum = 0;
    $(".programMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#programType").val(sum);
});

$(".userPreScreenMask").click(function () {
    var sum = 0;
    $(".userPreScreenMask").each(function () {
        if ($(this).prop("checked"))
            sum += parseInt($(this).val());
    });

    $("#userPreScreenAns").val(sum);
});