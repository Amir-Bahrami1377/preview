// #region Business

$(document).ready(function () {
    LoadExpert();
});

document.addEventListener("click", function (event) {
    if (event.target.id === "CreateExpert")
        CreateExpert();

    if (event.target.id === "AddOrUpdateExpert")
        ExpertFormValidatoin();

    let edit = event.target.closest(".ExpertEditRow");
    if (edit && edit.id) {
        var identity = document.getElementById(edit.id).getAttribute("data-identity");
        CreateExpert(identity);
    }

    let del = event.target.closest(".ExpertDeleteRow");
    if (del && del.id) {
        var identity = document.getElementById(del.id).getAttribute("data-identity");
        DeleteExpert(identity);
    }
});

function LoadExpert() {
    var shop = document.getElementById("shop").value;
    var shod = document.getElementById("shod").value;
    $.ajax({
        type: "GET",
        datatype: "JSON",
        url: '/Marahel/Expert/Index',
        data: { shop: shop, requestNumber: shod, message: "" },
        success: function (data) {
            document.getElementById("ExpertView").innerHTML = data;
        },
        error: function (data) {
            if (data.status === 429)
                window.location.href = "/Error/Error429";
            else
                NotyfNews(data.message, "error");
        }
    });
}

function CreateExpert(Id) {

    var id = Id ?? 0;
    var requestNumber = document.getElementById("ExpertRequestNumber").value;

    var model = new FormData();
    model.append("identity", id);
    model.append("requestNumber", requestNumber);

    var url = "Create";
    if (id > 0)
        url = "Edit";

    ShowModal(`/Marahel/Expert/${url}`, model);
}

function DeleteExpert(Id) {
    var requestNumber = document.getElementById("ExpertRequestNumber").value;
    var shop = document.getElementById("shop").value;

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: '/Marahel/Expert/Delete',
        data: {
            id: Id,
            requestNumber: requestNumber,
            shop: shop
        },
        success: function (data) {
            if (!data.success)
                data.message.forEach(msg => NotyfNews(msg, "error"));
            else {
                LoadExpert();
                NotyfNews(data.message, "success");
            }
        },
        error: function (data) {
            if (data.status === 429)
                window.location.href = "/Error/Error429";
            else
                NotyfNews(data.message, "error");
        }
    });
}

// #endregion

// #region Validation

function ExpertFormValidatoin() {
    var valid = true;

    var Identity = document.getElementById("Identity").value;
    var RequestNumber = document.getElementById("RequestNumber").value;
    if (!window.ValidationService.CheckRequired(RequestNumber, "شماره درخواست")) valid = false;
    if (!window.ValidationService.MoreThan(RequestNumber, 0, "شماره درخواست")) valid = false;

    var name = document.getElementById("Name").value;
    if (!window.ValidationService.CheckRequired(name, "نام")) valid = false;
    if (name != "" && !window.ValidationService.IsPersianLetters(name, "نام")) valid = false;
    if (name != "" && !window.ValidationService.SanitizeAndValidateInput(name, "نام")) valid = false;
    if (name != "" && !window.ValidationService.CheckMaxLength(name, 200, "نام")) valid = false;

    var family = document.getElementById("Family").value;
    if (!window.ValidationService.CheckRequired(family, "نام خانوادگی")) valid = false;
    if (family != "" && !window.ValidationService.IsPersianLetters(family, "نام خانوادگی")) valid = false;
    if (family != "" && !window.ValidationService.SanitizeAndValidateInput(family, "نام خانوادگی")) valid = false;
    if (family != "" && !window.ValidationService.CheckMaxLength(family, 200, "نام خانوادگی")) valid = false;

    var DateVisit = document.getElementById("DateVisit").value;
    if (DateVisit != "" && !window.ValidationService.IsValidPersianDate(DateVisit, "تاریخ بازدید")) valid = false;
    if (!window.ValidationService.CheckRequired(DateVisit, "تاریخ بازدید")) valid = false;

    if (!valid)
        return;

    var model = new FormData();
    model.append("Identity", Identity);
    model.append("RequestNumber", RequestNumber);
    model.append("Name", name);
    model.append("Family", family);
    model.append("DateVisit", DateVisit);

    var url = "CreateSubmit";
    if (Identity > 0)
        url = "EditSubmit";

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: `/Marahel/Expert/${url}`,
        data: model,
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.success)
                data.message.forEach(msg => NotyfNews(msg, "error"));
            else {
                CloseModal();
                LoadExpert();
                NotyfNews(data.message, "success");
            }
        },
        error: function (data) {
            if (data.status === 429)
                window.location.href = "/Error/Error429";
            else
                NotyfNews(data.message, "error");
        }
    });
}

// #endregion
