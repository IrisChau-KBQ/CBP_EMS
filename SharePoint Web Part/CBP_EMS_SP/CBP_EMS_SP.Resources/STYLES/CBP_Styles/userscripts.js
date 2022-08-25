
if (typeof( IsFullMask)=="undefined")
{
    IsFullMask = 0;
}
    if(IsFullMask==1)
    {
        $("#ems-customheader").hide();
    }else{
        $("body").addClass("background-theme emscontent overflowscroll");
        $("#ems-customheader").show();
        $("#sideNavBox").hide();
        $("#ems-userleft").show();
       

    }


    $(document).ready(function () {
        $("input[id*=txtLoginPassword]").on('input', function () {
          
            $(".text-danger").css("display", "none");
        });
            $("input[id*= txtRegEmail").on('input', function () {

                $(".text-danger").css("display", "none");
            });
            $("input[id*= txtRegPassword").on('input', function () {

                $(".text-danger").css("display", "none");
            });
            $("input[id*= txtRegPasswordConfirm").on('input', function () {

                $(".text-danger").css("display", "none");
            });
                
    });
        
//    try {datepickerYM hasDatepicker
//        var clientContext = new SP.ClientContext.get_current();
//        var tempcurrentUser = clientContext.get_web().get_currentUser();
//        clientContext.load(tempcurrentUser); datepickerYM hasDatepicker
//        clientContext.executeQueryAsync(function () {
//            console.log(tempcurrentUser.get_loginName());
//            var index = tempcurrentUser.get_loginName().lastIndexOf('|') + 1;
//            var currentUser = tempcurrentUser.get_loginName().substring(index);
//            $("#ems-cumstom-UserName").text(currentUser); // Here you will get name of the user

//        }, queryFailure);
//    }
//    catch (err) {
//        queryFailure();
//    }
//}

//function queryFailure () {
//    console.log("error occured while getting loggedin user name")
//}

//You can call the function like this
//SP.SOD.executeFunc('sp.js', 'SP.ClientContext', getCurrentUser);




//$("input[id*=rbtnPanel1Q4]").click(function () {
//    if ((this.value) == "True") {
//        $("input[id*=rbtnPanel1Q5]").attr("disabled", true);
//    }
//    else {
//        $("input[id*=rbtnPanel1Q5]").attr("disabled", false);
//    }
//    alert($("input[id*=rbtnPanel1Q5]").val());
//});

//$("input[id*=rbtnPanel1Q8]").click(function () {
//    if ((this.value) == "False") {
//        $("input[id*=rbtnPanel1Q9]").attr("disabled", true);
//    }
//    else {
//        $("input[id*=rbtnPanel1Q9]").attr("disabled", false);
//    }
//    alert($("input[id*=rbtnPanel1Q9]").val());
//});

$("input[id*=rbtnResubmission]").click(function () {

    if ((this.value) == "True") {
        $("textarea[id*=txtResubmission_Project_Reference]").attr("disabled", false);
        $("textarea[id*=txtResubmission_Main_Differences]").attr("disabled", false);
    }
    else {
        $("textarea[id*=txtResubmission_Project_Reference]").attr("disabled", true);
        $("textarea[id*=txtResubmission_Main_Differences]").attr("disabled", true);
    }
});

$(".rboCompany_Type input[type=radio]").click(function () {
    if ((this.value) == "Others") {
        $("input[id*=txtOther_Company_Type]").show();
    }
    else {
        $("input[id*=txtOther_Company_Type]").hide();
    }
});
$(".rdoBusiness_Area input[type=radio]").click(function () {
    if ((this.value) == "Others") {
        $("input[id*=txtOther_Bussiness_Area]").show();
    }
    else {
        $("input[id*=txtOther_Bussiness_Area]").hide();
    }
});

$(".chkPositioning input[type=checkbox]").click(function () {
   
    if ((this.value) == "Others") {
        if ($(this).is(":checked"))
            $("input[id*=txtPositioningOther]").show();
        else
            $("input[id*=txtPositioningOther]").hide();

    }
    else if ((this.value) == "Management / trading / service") {
        if ($(this).is(":checked"))
            $("input[id*=txtManagementOther]").show();
        else
            $("input[id*=txtManagementOther]").hide();

    }
});

$(".ProgramSubmitExists").click(function () {

    alert("You have already Apply. Please check My Application to continue.");
    return false;

});
