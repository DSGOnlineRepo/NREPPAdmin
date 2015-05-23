function selectAssignment(crtl)
{
    //alert($(crtl));
    $("#ValueString").val($(crtl).siblings("input[type=hidden]").val());

}