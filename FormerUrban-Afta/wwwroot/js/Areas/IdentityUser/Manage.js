document.addEventListener("click", function (event) {
    let enable = event.target.closest(".Enable2faRow");
    if (enable && enable.id) {
        var userName = document.getElementById(enable.id).getAttribute("data-username");
        EnableAuthenticator(userName);
    }

    let Disable = event.target.closest(".Disable2faRow");
    if (Disable && Disable.id) {
        var userName = document.getElementById(Disable.id).getAttribute("data-username");
        DisableAuthenticator(userName);
    }

    if (event.target.id === "EnableAuthenticatorSubmit") {
        EnableAuthenticatorSubmit();
    }

    if (event.target.id === "DisableAuthenticatorSubmit") {
        DisableAuthenticatorSubmit();
    }
});

function EnableAuthenticator(userName) {
    var model = new FormData();
    model.append("userName", userName);
    ShowModal('/Manage/EnableAuthenticator', model);
}

function EnableAuthenticatorSubmit() {
    
    var valid = true;
    var userName = document.getElementById("UserName").value;
    if (!window.ValidationService.CheckRequired(userName, "نام کاربری")) valid = false;

    var code = document.getElementById("Code").value;
    if (!window.ValidationService.CheckRequired(code, "کد OTP")) valid = false;
    if (code != "" && !ValidationService.IsDigitsOnly(code, "کد OTP")) valid = false;
    if (code != "" && !window.ValidationService.CheckLength(code, 6, "کد OTP")) valid = false;

    if (!valid) return;
        
    var model = new FormData();
    model.append("UserName", userName);
    model.append("Code", code);

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: `/Manage/EnableAuthenticatorSubmit`,
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

function DisableAuthenticator(userName) {
    var model = new FormData();
    model.append("userName", userName);
    ShowModal('/Manage/DisableAuthenticator', model);
}

function DisableAuthenticatorSubmit() {
    debugger;

    var valid = true;
    var userName = document.getElementById("UserName").value;
    if (!window.ValidationService.CheckRequired(userName, "نام کاربری")) valid = false;

    var code = document.getElementById("Code").value;
    if (!window.ValidationService.CheckRequired(code, "کد OTP")) valid = false;
    if (code != "" && !window.ValidationService.CheckLength(code, 6, "کد OTP")) valid = false;

    if (!valid) return;

    var model = new FormData();
    model.append("UserName", userName);
    model.append("Code", code);

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: `/Manage/DisableAuthenticatorSubmit`,
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