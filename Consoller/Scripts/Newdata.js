
function codeAddress() {
    LoadDocument()
    LoadHomeRecentPost()
}
window.onload = codeAddress;


// Load A Document on page load

// Get Document List
function LoadDocument() {

    var application = $("#application").val();
    debugger;
    var url = "/Application/GetDocument?id=" + application;
    // var url = "/Model/EmployeePartial";
    $("#document").load(url, function () {

        //$("#form-bp1").modal("show");

    })

};
 // Offer Letter List
function LoadHomeRecentPost() {

    var application = $("#application").val();
    debugger;
    var url = "/Application/GetOfferLetter?id=" + application;
    // var url = "/Model/EmployeePartial";
    $("#dvHomePopularPostBind").load(url, function () {

        //$("#form-bp1").modal("show");

    })

};


$(document).ready(function () {
    debugger;

    LoadDocument()
});

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
            "BankName": Bank, "Amount": Amount, "ApplicationNo": application,"Sms":Sms,"Show":View,"Email":Email
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

function SaveFile() {
    debugger;
    if (alertsubmit() == true)
    {
        var data = new FormData();
        var files = $("#Submitfile").get(0).files;
        var SubmitDate = $("#SubmitDate").val();
        var Uci = $("#Uci").val();
        var Appno = $("#Appno").val();
        var GcKey = $("#Gckey").val();
        var Password = $("#Password").val();
        var Question1 = $("#Q1").val();
        var Answer1 = $("#A1").val();
        var Question2 = $("#Q2").val();
        var Answer2 = $("#A2").val();
        var Question3 = $("#Q3").val();
        var Answer3 = $("#A3").val();
        var Question4 = $("#Q4").val();
        var Answer4 = $("#A4").val();
        var SubmitBy = $("#Submitby").val();
        var application = $("#application").val();
        var Sms = $("#filesms").is(":checked");
        var View = $("#fileView").is(":checked");
        var Email = $("#fileEmail").is(":checked");
        if (files.length > 0) {
            data.append("FileDoc", files[0]);

        }
        data.append("SubmitDate", SubmitDate);
        data.append("SubmitBy", SubmitBy);
        data.append("ApplicationNo", application);
        data.append("Sms", Sms);
        data.append("Show", View);
        data.append("Email", Email);
        data.append("Uci", Uci);
        data.append("Appno", Appno);
        data.append("Gckey", GcKey);
        data.append("Password", Password);
        data.append("Question1", Question1);
        data.append("Answer1", Answer1);
        data.append("Question2", Question2);
        data.append("Answer2", Answer2);
        data.append("Question3", Question3);
        data.append("Answer3", Answer3);
        data.append("Question4", Question4);
        data.append("Answer4", Answer4);
        $.ajax({
            url: "/Application/SubmitFile",
            type: "POST",
            processData: false,
            contentType: false,
            data: data,
            success: function (response) {
                //code after success
                LoadSubmit()
                $("#SubmitBy").val("");
                //    $("#imgPreview").attr('src', '/Upload/' + response);
                //  LoadDocument()
            },
            error: function (er) {
                alert(er);
            }

        });

    }
    else
    {

    }
   
}
function alertsubmit()
{
    var GcKey = $("#GcKey").val();
    var Password = $("#Password").val();
    var Question1 = $("#Q1").val();
    var Answer1 = $("#A1").val();
    var Question2 = $("#Q2").val();
    var Answer2 = $("#A2").val();
    var Question3 = $("#Q3").val();
    var Answer3 = $("#A3").val();
    var Question4 = $("#Q4").val();
    var Answer4 = $("#A4").val();
    if (GcKey == "" || Password == "" || Question1 == "" || Answer1 == "" || Question2 == "" || Answer2 == "" || Question3 == "" || Answer3 == "" || Question4 == "" || Answer4 == "")
    {
        alert("Please Enter Gic Full Details");
        return false;
    }
    return true;
}

function SaveMedical() {


    var Date = $("#Date").val();
    var Hospital = $("#Hospital").val();
    var Note = $("#Note").val();
    var application = $("#application").val();
    var Sms = $("#medsms").is(":checked");
    var View = $("#medView").is(":checked");
    var Email = $("#medEmail").is(":checked");
    var Med = {
        "Date": Date, "HospitalName": Hospital, "ApplicationNo": application, "Note": Note, "Sms": Sms, "Show": View, "Email": Email
    };
    debugger;
    $.post("/Application/Medical", Med,
    function (data2) {

        LoadMedical()
        if (data2 == 0) {
            location = location.href;
        }
    }, 'json');



}

function LoadMedical() {

    var application = $("#application").val();
    debugger;
    var url = "/Application/GetMedical?id=" + application;
    // var url = "/Model/EmployeePartial";
    $("#Medical").load(url, function () {

        //$("#form-bp1").modal("show");

    })

};
function LoadGic() {

    var application = $("#application").val();
    debugger;
    var url = "/Application/GetGic?id=" + application;
    // var url = "/Model/EmployeePartial";
    $("#Gic").load(url, function () {

        //$("#form-bp1").modal("show");

    })

};
function LoadSubmit() {

    var application = $("#application").val();
    debugger;
    var url = "/Application/GetSub?id=" + application;
    // var url = "/Model/EmployeePartial";
    $("#Sub").load(url, function () {

        //$("#form-bp1").modal("show");

    })

};

function AlertGic() {

    debugger;
    var Date = $("#GicDate").val();
    var Account = $("#Account").val();
    var Bank = $("#Bank").val();
    var Amount = $("#Amount").val();
    if(date=="")
    {
        alert("select Date");
    }
   
    };


