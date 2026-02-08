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
            $(".EventLogThresholdReadOnly").prop("disabled", true);
            $(".EventLogThresholdReadOnly").prop("readonly", true);

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

document.addEventListener("submit", function (event) {
    if (event.target.id === "EventLogThresholdForm") {
        EventLogThresholdFormValidation(event);
    }
})

function Edit() {
    document.getElementById("Cancel").classList.remove("d-none");
    document.getElementById("Submit").classList.remove("d-none");
    document.getElementById("Edit").classList.add("d-none");
    $(".EventLogThresholdReadOnly").prop("disabled", false);
    $(".EventLogThresholdReadOnly").prop("readonly", false);
}

function Cancel() {
    document.getElementById("messages").value = "";
    window.location.href = "/Setting/EventLogThreshold";
}

function EventLogThresholdFormValidation(e) {
    var valid = true;

    var UsersLoginLogWarning = document.getElementById("UsersLoginLogWarning").value;
    if (!window.ValidationService.MoreThan2(UsersLoginLogWarning, 999, "حد آستانه‌ی هشدار برای حجم جدول ذخیره‌سازی لاگ ورود/خروج کاربران")) valid = false;

    var UsersLoginLogCritical = document.getElementById("UsersLoginLogCritical").value;
    if (!window.ValidationService.MoreThan2(UsersLoginLogCritical, 1099, "حد آستانه‌ی بحرانی برای حجم جدول ذخیره‌سازی لاگ ورود/خروج کاربران")) valid = false;

    if (parseInt(UsersLoginLogWarning) >= parseInt(UsersLoginLogCritical)) {
        NotyfNews("مقدار حد آستانه ی هشدار برای حجم جدول ذخیره سازی لاگ ورود/خروج کاربران باید کمتر از مقدار حد آستانه ی بحرانی برای حجم جدول ذخیره سازی لاگ ورود/خروج کاربران باشد.", "error");
        valid = false;
    }

    var UsersActivityLogWarning = document.getElementById("UsersActivityLogWarning").value;
    if (!window.ValidationService.MoreThan2(UsersActivityLogWarning, 999, "حد آستانه‌ی هشدار برای حجم جدول ذخیره‌سازی لاگ فعالیت کاربران")) valid = false;

    var UsersActivityLogCritical = document.getElementById("UsersActivityLogCritical").value;
    if (!window.ValidationService.MoreThan2(UsersActivityLogCritical, 1099, "حد آستانه‌ی بحرانی برای حجم جدول ذخیره‌سازی لاگ فعالیت کاربران")) valid = false;

    if (parseInt(UsersActivityLogWarning) >= parseInt(UsersActivityLogCritical)) {
        NotyfNews("مقدار حد آستانه ی هشدار برای حجم جدول ذخیره سازی لاگ فعالیت کاربران باید کمتر از مقدار حد آستانه ی بحرانی برای حجم جدول ذخیره سازی لاگ فعالیت کاربران باشد.", "error");
        valid = false;
    }

    var UsersAuditsLogWarning = document.getElementById("UsersAuditsLogWarning").value;
    if (!window.ValidationService.MoreThan2(UsersAuditsLogWarning, 999, "حد آستانه‌ی هشدار برای حجم جدول ذخیره‌سازی لاگ تغییرات داده های سیستم")) valid = false;

    var UsersAuditsLogCritical = document.getElementById("UsersAuditsLogCritical").value;
    if (!window.ValidationService.MoreThan2(UsersAuditsLogCritical, 1099, "حد آستانه‌ی بحرانی برای حجم جدول ذخیره‌سازی لاگ تغییرات داده های سیستم")) valid = false;

    if (parseInt(UsersAuditsLogWarning) >= parseInt(UsersAuditsLogCritical)) {
        NotyfNews("مقدار حد آستانه ی هشدار برای حجم جدول ذخیره سازی لاگ تغییرات داده های سیستم باید کمتر از مقدار حد آستانه ی بحرانی برای حجم جدول ذخیره سازی لاگ تغییرات داده های سیستم باشد.", "error");
        valid = false;
    }

    if (!valid)
        e.preventDefault();
}