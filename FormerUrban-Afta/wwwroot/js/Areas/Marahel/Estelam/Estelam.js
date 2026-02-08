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
            $(".EstelamReadOnly").prop("disabled", true);
            $(".EstelamReadOnly").prop("readonly", true);

            var cancel = document.getElementById("Cancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("Submit");
            if (submit)
                submit.classList.add("d-none");
        }
    }

    ShowDate();
    DisabledDate();
});

document.addEventListener("click", function (event) {
    if (event.target.id === "Edit") {
        Edit(event);
    }
    if (event.target.id === "Cancel") {
        Cancel(event);
    }
    if (event.target.id === "NoeMalekiatBtn") {
        ShowParametric('EnumNoemalekiat', 'codeNoeMalekiat', 'NoeMalekiat');
    }
});

function Edit() {
    document.getElementById("Cancel").classList.remove("d-none");
    document.getElementById("Submit").classList.remove("d-none");
    document.getElementById("Edit").classList.add("d-none");
    $(".EstelamReadOnly").prop("disabled", false);
    $(".EstelamReadOnly").prop("readonly", false);
    EnabledDate();
}

function Cancel() {
    document.getElementById("messages").value = "";
    var codeMarhaleh = document.getElementById("codeMarhaleh").value;
    var shop = document.getElementById("shop").value;
    var sh_Darkhast = document.getElementById("Sh_Darkhast").value;
    window.location.href = "/Marahel/Estelam?shop=" + encodeURIComponent(shop) + "&shod=" + encodeURIComponent(sh_Darkhast) + "&codeMarhaleh=" + encodeURIComponent(codeMarhaleh);
}

// #endregion

// #region Validation
document.addEventListener("submit", function (event) {
    if (event.target.id === "EstelamForm")
        EstelamFormValidatoin(event);
});

function EstelamFormValidatoin(e) {
    var valid = true;
    EnabledDate();

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده")) valid = false;

    var shod = document.getElementById("shod").value;
    if (!window.ValidationService.CheckRequired(shod, "شماره درخواست")) valid = false;
    if (!window.ValidationService.NotEqual(shod, 0, "شماره درخواست")) valid = false;

    var codeNoeMalekiat = document.getElementById("codeNoeMalekiat").value;
    if (!window.ValidationService.CheckRequired(codeNoeMalekiat, "نوع مالکیت")) valid = false;
    if (codeNoeMalekiat != "" && !window.ValidationService.MoreThan(codeNoeMalekiat, 0, "نوع مالکیت")) valid = false;

    var NoeMalekiat = document.getElementById("NoeMalekiat").value;
    if (!window.ValidationService.isAlphanumeric(NoeMalekiat, "نوع مالکیت")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(NoeMalekiat, "نوع مالکیت")) valid = false;
    if (!window.ValidationService.CheckMaxLength(NoeMalekiat, 200, "نوع مالکیت")) valid = false;

    var Tarikh_Pasokh = document.getElementById("Tarikh_Pasokh").value;
    if (!window.ValidationService.CheckRequired(Tarikh_Pasokh, "تاریخ پاسخ استعلام")) valid = false;
    if (Tarikh_Pasokh != "" && !window.ValidationService.IsValidPersianDate(Tarikh_Pasokh, "تاریخ پاسخ استعلام")) valid = false;

    var Sh_Pasokh = document.getElementById("Sh_Pasokh").value;
    if (!window.ValidationService.CheckRequired(Sh_Pasokh, "شماره پاسخ استعلام")) valid = false;
    if (Sh_Pasokh != "" && !window.ValidationService.isAlphanumeric(Sh_Pasokh, "شماره پاسخ استعلام")) valid = false;
    if (Sh_Pasokh != "" && !window.ValidationService.SanitizeAndValidateInput(Sh_Pasokh, "شماره پاسخ استعلام")) valid = false;
    if (Sh_Pasokh != "" && !window.ValidationService.CheckMaxLength(Sh_Pasokh, 200, "شماره پاسخ استعلام")) valid = false;

    var Dang_Enteghal = document.getElementById("Dang_Enteghal").value;
    if (!window.ValidationService.CheckRequired(Dang_Enteghal, "دانگ مورد انتقال")) valid = false;
    if (Dang_Enteghal != "" && !ValidationService.IsDigitsOnly(Dang_Enteghal, "دانگ مورد انتقال")) valid = false;
    if (Dang_Enteghal != "" && !ValidationService.Between(Dang_Enteghal, 0, 6, "دانگ مورد انتقال")) valid = false;

    var Kharidar = document.getElementById("Kharidar").value;
    if (!window.ValidationService.CheckRequired(Kharidar, "نام خریدار")) valid = false;
    if (Kharidar != "" && !window.ValidationService.IsPersianLetters(Kharidar, "نام خریدار")) valid = false;
    if (Kharidar != "" && !window.ValidationService.SanitizeAndValidateInput(Kharidar, "نام خریدار")) valid = false;
    if (Kharidar != "" && !window.ValidationService.CheckMaxLength(Kharidar, 200, "نام خریدار")) valid = false;

    var Tozihat = document.getElementById("Tozihat").value;
    if (!window.ValidationService.CheckRequired(Tozihat, "توضیحات")) valid = false;
    if (!window.ValidationService.isAlphanumeric(Tozihat, "توضیحات")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(Tozihat, "توضیحات")) valid = false;
    if (Tozihat != "" && !window.ValidationService.CheckMaxLength(Tozihat, 500, "توضیحات")) valid = false;

    if (!valid)
        e.preventDefault();
}

// #endregion