// #region Business

$(document).ready(function () {
    var errorElement = document.getElementById("messages");

    if (errorElement) { // بررسی وجود المنت
        var messageJson = errorElement.value;
        var messages = messageJson ? JSON.parse(messageJson) : [];

        if (messages.length > 0) {
            messages.forEach(msg => NotyfNews(msg, "error"));
            document.getElementById("Edit").classList.add("d-none");
        }
        else {
            $(".SmsSettingReadOnly").prop("disabled", true);
            $(".SmsSettingReadOnly").prop("readonly", true);

            var cancel = document.getElementById("Cancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("Submit");
            if (submit)
                submit.classList.add("d-none");
        }
    }
});

document.addEventListener("click", function (event) {
    if (event.target.id === "Edit") {
        Edit(event);
    }
    if (event.target.id === "Cancel") {
        Cancel(event);
    }
});

function Edit() {
    document.getElementById("Cancel").classList.remove("d-none");
    document.getElementById("Submit").classList.remove("d-none");
    document.getElementById("Edit").classList.add("d-none");
    $(".SmsSettingReadOnly").prop("disabled", false);
    $(".SmsSettingReadOnly").prop("readonly", false);
}

function Cancel() {
    document.getElementById("messages").value = "";
    window.location.href = "/Setting/SmsSetting";
}

// #endregion

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "SmsSettingForm")
        FormValidatoin(event);
});

function FormValidatoin(e) {
    var valid = true;
    
    var sms_user = document.getElementById("sms_user").value;
    if (!window.ValidationService.CheckRequired(sms_user, "نام کاربری")) valid = false;
    if (sms_user != "" && !window.ValidationService.CheckMinLength(sms_user, 3, "نام کاربری")) valid = false;
    if (sms_user != "" && !window.ValidationService.CheckMaxLength(sms_user, 30, "نام کاربری")) valid = false;

    var sms_pass = document.getElementById("sms_pass").value;
    if (!window.ValidationService.CheckRequired(sms_pass, "رمز عبور")) valid = false;
    if (sms_pass != "" && !window.ValidationService.CheckMinLength(sms_pass, 3, "رمز عبور")) valid = false;
    if (sms_pass != "" && !window.ValidationService.CheckMaxLength(sms_pass, 128, "رمز عبور")) valid = false;

    if (!valid)
        e.preventDefault();
}

// #endregion