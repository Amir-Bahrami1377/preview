document.addEventListener("click", function (event) {
    if (event.target.id === "CreateRoleRestriction")
        CreateRoleRestriction("");

    if (event.target.id === "CreateRoleRestrictionSubmit")
        RoleRestrictionFormValidatoin("");

    if (event.target.id === "EditRoleRestrictionSubmit")
        RoleRestrictionFormValidatoin("");


    let item = event.target.closest(".RoleRestrictionEditRow");
    if (item && item.id) {
        var identity = document.getElementById(item.id).getAttribute("data-identity");
        CreateRoleRestriction(identity);
    }

    let item2 = event.target.closest(".RoleRestrictionDeleteRow");
    if (item2 && item2.id) {
        var identity = document.getElementById(item2.id).getAttribute("data-identity");
        DeleteRoleRestriction(identity);
    }
});

function CreateRoleRestriction(Id) {

    var id = Id ?? "";

    var model = new FormData();
    model.append("identity", id);

    var url = Id > 0 ? "Edit" : "Create";

    ShowModal(`/Setting/RoleRestriction/${url}`, model);
}

function RoleRestrictionFormValidatoin() {
    var valid = true;

    var Identity = document.getElementById('Identity').value;

    var RoleId = document.getElementById("RoleId").value;
    if (RoleId != "" && !window.ValidationService.SanitizeAndValidateInput(RoleId, "نقش")) valid = false;
    if (RoleId != "" && !window.ValidationService.CheckMaxLength(RoleId, 200, "نقش")) valid = false;

    var RoleText = $("#RoleId option:selected").text();
    if (RoleText != "" && !window.ValidationService.SanitizeAndValidateInput(RoleId, "نقش")) valid = false;
    if (RoleText != "" && !window.ValidationService.CheckMaxLength(RoleId, 200, "نقش")) valid = false;

    var FromDate = document.getElementById('FromDate').value;
    if (!window.ValidationService.CheckRequired(FromDate, "تاریخ شروع")) valid = false;
    if (!window.ValidationService.isValidPersianDateTime(FromDate, "تاریخ شروع")) valid = false;

    var ToDate = document.getElementById('ToDate').value;
    if (!window.ValidationService.CheckRequired(ToDate, "تاریخ پایان")) valid = false;
    if (!window.ValidationService.isValidPersianDateTime(ToDate, "تاریخ پایان")) valid = false;

    var Description = document.getElementById('Description').value;
    if (!window.ValidationService.CheckRequired(Description, "توضیحات")) valid = false;
    if (!window.ValidationService.isAlphanumeric(Description, "توضیحات")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(Description, "توضیحات")) valid = false;
    if (Description != "" && !window.ValidationService.CheckMaxLength(Description, 500, "توضیحات")) valid = false;

    if (!valid)
        return;

    var model = new FormData();
    model.append("Identity", Identity);
    model.append("RoleId", RoleId);
    model.append("RoleText", RoleText);
    model.append("FromDate", toEnglishDigits(FromDate));
    model.append("ToDate", toEnglishDigits(ToDate));
    model.append("Description", Description);

    var url = "CreateSubmit";
    if (Identity > 0)
        url = "EditSubmit";

    $.ajax({
        type: "POST",
        url: `/Setting/RoleRestriction/${url}`,
        data: model,
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.success)
                if (Array.isArray(data.message)) {
                    data.message.forEach(msg => NotyfNews(msg, "error"));
                } else {
                    NotyfNews(data.message, "error");
                }
            else {
                CloseModal('1');
                location.reload();
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

function DeleteRoleRestriction(id) {
    $.ajax({
        type: "Post",
        url: `/Setting/RoleRestriction/Delete`,
        data: { id: id },
        success: function (data) {
            if (!data.success)
                if (Array.isArray(data.message)) {
                    data.message.forEach(msg => NotyfNews(msg, "error"));
                } else {
                    NotyfNews(data.message, "error");
                }
            else {
                location.reload();
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