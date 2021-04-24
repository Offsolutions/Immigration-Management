function SaveGic() {


    var Date = $("#GicDate").val();
    var Account = $("#Account").val();
    var Bank = $("#Bank").val();
    var Amount = $("#Amount").val();
    var application = $("#application").val();

    var Sms = $("#gicsms").is(":checked");
    var View = $("#gicView").is(":checked");
    var Email = $("#gicEmail").is(":checked");
    var Employee = {
        "Date": Date, "AccountNo": Account,
        "BankName": Bank, "Amount": Amount, "ApplicationNo": application, "Sms": Sms, "Show": View, "Email": Email
    };
    debugger;
    $.post("/Application/Gic", Employee,
    function (data) {
        debugger;
        LoadGic()
        //$("#countryid").val("");
        //$("#cityDropDown").val("");
        //$("#ddlcourse").val("");
        //$("#Amount").val("");

        //$("#Month").val("");
        //$("#Year").val("");

        if (data == 0) {
            location = location.href;
        }
    }, 'json');



}