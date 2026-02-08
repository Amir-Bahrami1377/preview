// #region Business

document.addEventListener("click", function (event) {
    if (event.target.id === "CreateIp")
        CreateWhiteIp("");

    if (event.target.id === "CreateIpBtnSubmit")
        IpFormValidatoin();

    if (event.target.id === "EditIpBtnSubmit")
        IpFormValidatoin();

    let item = event.target.closest(".IpEditRow");
    if (item && item.id) {
        var identity = document.getElementById(item.id).getAttribute("data-identity");
        CreateWhiteIp(identity);
    }

    let item2 = event.target.closest(".IpDeleteRow");
    if (item2 && item2.id) {
        var identity = document.getElementById(item2.id).getAttribute("data-identity");
        DeleteWhiteIp(identity);
    }
});

function CreateWhiteIp(Id) {

    var id = Id ?? "";

    var model = new FormData();
    model.append("identity", id);

    var url = Id > 0 ? "EditWhiteIp" : "CreateWhiteIp";

    ShowModal(`/Setting/IPWhiteList/${url}`, model);
}

function DeleteWhiteIp(id) {
    $.ajax({
        type: "GET",
        url: `/Setting/IPWhiteList/DeleteRow`,
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

// #endregion

function IpFormValidatoin() {
    var valid = true;
    
    var Identity = document.getElementById('Identity').value;

    var IPRange = document.getElementById('IPRange').value;
    if (!window.ValidationService.CheckRequired(IPRange, "رنج آی پی")) valid = false;
    if (IPRange != "" && !window.ValidationService.isValidIpRangeOrCidr(IPRange)) valid = false;

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

    if(!valid)
        return;
    
    var model = new FormData();
    model.append("Identity", Identity);
    model.append("IPRange", IPRange);
    model.append("FromDate", toEnglishDigits(FromDate));
    model.append("ToDate", toEnglishDigits(ToDate));
    model.append("Description", Description);

    var url = "CreateWhiteIpSubmit";
    if(Identity > 0)
        url = "EditWhiteIpSubmit";

    $.ajax({
        type: "POST",
        url: `/Setting/IPWhiteList/${url}`,
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
