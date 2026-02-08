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
            $(".LoginSettingReadOnly").prop("disabled", true);
            $(".LoginSettingReadOnly").prop("readonly", true);

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
    $(".LoginSettingReadOnly").prop("disabled", false);
    $(".LoginSettingReadOnly").prop("readonly", false);
}

function Cancel() {
    document.getElementById("messages").value = "";
    var url = document.getElementById("PageLink").value;
    window.location.href = "/Setting/LoginSetting/" + url;
}

// #endregion

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "LoginSettingForm")
        LoginSettingFormValidatoin(event);
});


// Login Setting
function LoginSettingFormValidatoin(e) {
    var valid = true;

    var MaximumSessions = document.getElementById("MaximumSessions").value;
    if (!window.ValidationService.CheckRequired(MaximumSessions, "حداکثر تعداد نشست")) valid = false;
    if (MaximumSessions != "" && !ValidationService.IsDigitsOnly(MaximumSessions, "حداکثر تعداد نشست")) valid = false;
    if (MaximumSessions != "" && !ValidationService.Between(MaximumSessions, 1, 10, "حداکثر تعداد نشست")) valid = false;

    var RetryLoginCount = document.getElementById("RetryLoginCount").value;
    if (!window.ValidationService.CheckRequired(RetryLoginCount, "تلاش برای لاگین ناموفق")) valid = false;
    if (RetryLoginCount != "" && !ValidationService.IsDigitsOnly(RetryLoginCount, "تلاش برای لاگین ناموفق")) valid = false;
    if (RetryLoginCount != "" && !ValidationService.Between(RetryLoginCount, 1, 10, "تلاش برای لاگین ناموفق")) valid = false;

    var PasswordLength = document.getElementById("PasswordLength").value;
    if (!window.ValidationService.CheckRequired(PasswordLength, "طول رمز عبور")) valid = false;
    if (PasswordLength != "" && !ValidationService.IsDigitsOnly(PasswordLength, "طول رمز عبور")) valid = false;
    if (PasswordLength != "" && !window.ValidationService.Between(PasswordLength, 12, 128, "طول رمز عبور")) valid = false;

    var KhatemeSessionAfterMinute = document.getElementById("KhatemeSessionAfterMinute").value;
    if (!window.ValidationService.CheckRequired(KhatemeSessionAfterMinute, "خاتمه نشست (دقیقه)")) valid = false;
    if (KhatemeSessionAfterMinute != "" && !ValidationService.IsDigitsOnly(KhatemeSessionAfterMinute, "خاتمه نشست (دقیقه)")) valid = false;
    if (!window.ValidationService.MoreThan(KhatemeSessionAfterMinute, 0, "خاتمه نشست (دقیقه)")) valid = false;

    var MaximumAccessDenied = document.getElementById("MaximumAccessDenied").value;
    if (!window.ValidationService.CheckRequired(MaximumAccessDenied, "حداکثر تعداد درخواست دسترسی رد شده")) valid = false;
    if (MaximumAccessDenied != "" && !ValidationService.IsDigitsOnly(MaximumAccessDenied, "حداکثر تعداد درخواست دسترسی رد شده")) valid = false;
    if (MaximumAccessDenied != "" && !window.ValidationService.Between(MaximumAccessDenied, 1, 10, "حداکثر تعداد درخواست دسترسی رد شده")) valid = false;

    var RequestRateLimitter = document.getElementById("RequestRateLimitter").value;
    if (!window.ValidationService.CheckRequired(RequestRateLimitter, "حداکثر تعداد درخواست در 30 ثانیه")) valid = false;
    if (RequestRateLimitter != "" && !ValidationService.IsDigitsOnly(RequestRateLimitter, "حداکثر تعداد درخواست در 30 ثانیه")) valid = false;
    if (RequestRateLimitter != "" && !window.ValidationService.Between(RequestRateLimitter, 10, 200, "حداکثر تعداد درخواست در 30 ثانیه")) valid = false;

    var UnblockingUserTime = document.getElementById("UnblockingUserTime").value;
    if (!window.ValidationService.CheckRequired(UnblockingUserTime, "رفع مسدود بودن کاربر")) valid = false;
    if (UnblockingUserTime != "" && !ValidationService.IsDigitsOnly(UnblockingUserTime, "رفع مسدود بودن کاربر")) valid = false;
    if (UnblockingUserTime != "" && !window.ValidationService.Between(UnblockingUserTime, 1, 525600, "رفع مسدود بودن کاربر")) valid = false;

    if (!valid)
        e.preventDefault();
}
