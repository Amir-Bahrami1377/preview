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
            $(".ControlMapReadOnly").prop("disabled", true);
            $(".ControlMapReadOnly").prop("readonly", true);

            var cancel = document.getElementById("Cancel");
            if(cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("Submit");
            if(submit)
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
    
    if (event.target.id === "NoenamaBtn") {
        ShowParametric('EnumNoename', 'C_NoeNama', 'NoeNama');
    }
    
    if (event.target.id === "NoesaghfBtn") {
        ShowParametric('EnumNoeSaghf', 'c_noesaghf', 'noesaghf');
    }
    
    if (event.target.id === "NoeSazeBtn") {
        ShowParametric('EnumNoeSaze', 'c_NoeSaze', 'NoeSaze');
    }

});

function Edit() {
    document.getElementById("Cancel").classList.remove("d-none");
    document.getElementById("Submit").classList.remove("d-none");
    document.getElementById("Edit").classList.add("d-none");
    $(".ControlMapReadOnly").prop("disabled", false);
    $(".ControlMapReadOnly").prop("readonly", false);
}

function Cancel() {
    document.getElementById("messages").value = "";
    var codeMarhaleh = document.getElementById("codeMarhaleh").value;
    var shop = document.getElementById("shop").value;
    var sh_Darkhast = document.getElementById("sh_Darkhast").value;
    window.location.href = "/Marahel/ControlMap?shop=" + encodeURIComponent(shop) + "&shod=" + encodeURIComponent(sh_Darkhast) + "&codeMarhaleh=" + encodeURIComponent(codeMarhaleh);
}

// #endregion

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "ControlMapForm")
        ControlMapFormValidatoin(event);
});

function ControlMapFormValidatoin(e) {

    var valid = true;

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده")) valid = false;

    var shod = document.getElementById("shod").value;
    if (!window.ValidationService.CheckRequired(shod, "شماره درخواست")) valid = false;
    if (!window.ValidationService.NotEqual(shod, 0, "شماره درخواست")) valid = false;

    var c_noenama = document.getElementById("C_NoeNama").value;
    if (!window.ValidationService.CheckRequired(c_noenama, "نوع نما")) valid = false;
    if (c_noenama != "" && !window.ValidationService.MoreThan(c_noenama, 0, "نوع نما")) valid = false;

    var noenama = document.getElementById("NoeNama").value;
    if (!window.ValidationService.isAlphanumeric(noenama, "نوع نما")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noenama, "نوع نما")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noenama, 200, "نوع نما")) valid = false;

    var c_noesaghf = document.getElementById("c_noesaghf").value;
    if (!window.ValidationService.CheckRequired(c_noesaghf, "نوع سقف")) valid = false;
    if (c_noesaghf != "" && !window.ValidationService.MoreThan(c_noesaghf, 0, "نوع سقف")) valid = false;

    var noesaghf = document.getElementById("noesaghf").value;
    if (!window.ValidationService.isAlphanumeric(noesaghf, "نوع سقف")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noesaghf, "نوع سقف")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noesaghf, 200, "نوع سقف")) valid = false;

    var c_NoeSaze = document.getElementById("c_NoeSaze").value;
    if (!window.ValidationService.CheckRequired(c_NoeSaze, "نوع سازه")) valid = false;
    if (c_NoeSaze != "" && !window.ValidationService.MoreThan(c_NoeSaze, 0, "نوع سازه")) valid = false;

    var NoeSaze = document.getElementById("NoeSaze").value;
    if (!window.ValidationService.isAlphanumeric(NoeSaze, "نوع سازه")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(NoeSaze, "نوع سازه")) valid = false;
    if (!window.ValidationService.CheckMaxLength(NoeSaze, 200, "نوع سازه")) valid = false;

    var masahat_s = document.getElementById("masahat_s").value;
    if (!ValidationService.AmountIsValidFormat(masahat_s, "مساحت طبق سند")) valid = false;
    if (!ValidationService.Between(masahat_s, 0, 1000000000, "مساحت طبق سند")) valid = false;

    var masahat_e = document.getElementById("masahat_e").value;
    if (!ValidationService.AmountIsValidFormat(masahat_e, "مساحت اصلاحی")) valid = false;
    if (!ValidationService.Between(masahat_e, 0, 1000000000, "مساحت اصلاحی")) valid = false;

    var masahat_m = document.getElementById("masahat_m").value;
    if (!ValidationService.AmountIsValidFormat(masahat_m, "مساحت موجود")) valid = false;
    if (!ValidationService.Between(masahat_m, 0, 1000000000, "مساحت موجود")) valid = false;

    var masahat_b = document.getElementById("masahat_b").value;
    if (!ValidationService.AmountIsValidFormat(masahat_b, "مساحت باقیمانده")) valid = false;
    if (!ValidationService.Between(masahat_b, 0, 1000000000, "مساحت باقیمانده")) valid = false;

    var satheshghal = document.getElementById("satheshghal").value;
    if (satheshghal != "" && !ValidationService.AmountIsValidFormat(satheshghal, "سطح اشغال")) valid = false;
    if (satheshghal != "" && !ValidationService.Between(satheshghal, 0, 1000000000, "سطح اشغال")) valid = false;

    var tarakom = document.getElementById("tarakom").value;
    if (tarakom != "" && !ValidationService.AmountIsValidFormat(tarakom, "مساحت مجاز در پروانه")) valid = false;
    if (tarakom != "" && !ValidationService.Between(tarakom, 0, 1000000000, "مساحت مجاز در پروانه")) valid = false;

    var TedadTabaghe = document.getElementById("TedadTabaghe").value;
    if (TedadTabaghe != "" && !ValidationService.Between(TedadTabaghe, 0, 99, "تعداد طبقه")) valid = false;

    if(!valid)
        e.preventDefault();
}

// #endregion