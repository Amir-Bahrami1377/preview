// #region Business

$(document).ready(function () {
    var errorElement = document.getElementById("Message");

    if (errorElement) {
        var messageJson = errorElement.value;
        var messages = messageJson ? JSON.parse(messageJson) : [];

        if (messages.length > 0)
            messages.forEach(msg => NotyfNews(msg, "error"));
    }

});

document.addEventListener("click", function (event) {
    if (event.target.id === "NoeMotBtn") {
        ShowParametric('EnumNoeMoteqazi', 'c_noemot', 'noemot');
    }
});

// #endregion

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "SabtDarkhastForm")
        SabtDarkhastFormValidatoin(event);
});

function SabtDarkhastFormValidatoin(event) {
    var valid = true;

    var shodarkhast = document.getElementById("shodarkhast").value;
    if (!window.ValidationService.CheckRequired(shodarkhast, "شماره درخواست")) valid = false;
    if (!window.ValidationService.MoreThan(shodarkhast, 0, "شماره درخواست")) valid = false;

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده")) valid = false;
    if (!window.ValidationService.MoreThan(shop, 0, "شماره پرونده")) valid = false;

    var c_nosazi = document.getElementById("c_nosazi").value;
    if (!ValidationService.IsValidCodeNosazi(c_nosazi)) valid = false;

    var c_noemot = document.getElementById("c_noemot").value;
    if (!window.ValidationService.CheckRequired(c_noemot, " نوع متقاضی")) valid = false;
    if (!window.ValidationService.MoreThan(c_noemot, 0, " نوع منقاضی")) valid = false;

    var noemot = document.getElementById("noemot").value;
    if (!window.ValidationService.isAlphanumeric(noemot, "نوع متقاضی")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noemot, "نوع متقاضی")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noemot, 200, "نوع متقاضی")) valid = false;

    var c_noedarkhast = document.getElementById("c_noedarkhast").value;
    if (!window.ValidationService.CheckRequired(c_noedarkhast, "نوع درخواست")) valid = false;
    if (!window.ValidationService.MoreThan(c_noedarkhast, 0, "نوع درخواست")) valid = false;

    var moteghazi = document.getElementById("moteghazi").value;
    if (!window.ValidationService.CheckRequired(moteghazi, "نام متقاضی")) valid = false;
    if (!window.ValidationService.IsPersianLetters(moteghazi, "نام متقاضی")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(moteghazi, "نام متقاضی")) valid = false;
    if (!window.ValidationService.CheckMaxLength(moteghazi, 200, "نام متقاضی")) valid = false;

    var CodeMeli = document.getElementById("CodeMeli").value;
    if (!window.ValidationService.CheckRequired(CodeMeli, "کد ملی")) valid = false;
    if (CodeMeli != "" && !window.ValidationService.IsValidNationalCode(CodeMeli)) valid = false;

    var tel = document.getElementById("tel").value;
    if (tel != "" && !ValidationService.IsValidTel(tel)) valid = false;

    var mob = document.getElementById("mob").value;
    if (!window.ValidationService.CheckRequired(mob, "شماره همراه")) valid = false;
    if (mob != "" && !ValidationService.IsValidMobileNumber(mob)) valid = false;

    var codeposti = document.getElementById("codeposti").value;
    if (codeposti != "" && !ValidationService.IsValidPostalCode(codeposti)) valid = false;

    var email = document.getElementById("email").value;
    if (email != "" && !ValidationService.IsValidEmail(email)) valid = false;
    if (email != "" && !ValidationService.CheckMaxLength(email, 320, "ایمیل")) valid = false;
    if (email != "" && !window.ValidationService.SanitizeAndValidateInput(email, "ایمیل")) valid = false;

    var address = document.getElementById("address").value;
    if (!window.ValidationService.CheckRequired(address, "آدرس")) valid = false;
    if (address != "" && !window.ValidationService.SanitizeAndValidateInput(address, "آدرس")) valid = false;
    if (address != "" && !window.ValidationService.CheckMaxLength(address, 500, "آدرس")) valid = false;

    if (!valid)
        event.preventDefault();
}

// #endregion