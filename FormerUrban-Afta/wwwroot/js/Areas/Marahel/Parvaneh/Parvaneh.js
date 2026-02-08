// #region business

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
            $(".ParvanehReadOnly").prop("disabled", true);
            $(".ParvanehReadOnly").prop("readonly", true);

            var cancel = document.getElementById("Cancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("Submit");
            if (submit)
                submit.classList.add("d-none");
        }
    }

    ShowDate();

});

document.addEventListener("click", function (event) {
    if (event.target.id === "Edit") {
        Edit(event);
    }
    if (event.target.id === "Cancel") {
        Cancel(event);
    }
    if (event.target.id === "noe_parvanehBtn") {
        ShowParametric('EnumNoeParvaneh', 'c_noeParvaneh', 'noe_parvaneh');
    }
});

function Edit() {
    document.getElementById("Cancel").classList.remove("d-none");
    document.getElementById("Submit").classList.remove("d-none");
    document.getElementById("Edit").classList.add("d-none");
    $(".ParvanehReadOnly").prop("disabled", false);
    $(".ParvanehReadOnly").prop("readonly", false);
}

function Cancel() {
    document.getElementById("messages").value = "";
    var codeMarhaleh = document.getElementById("codeMarhaleh").value;
    var shop = document.getElementById("shop").value;
    var sh_Darkhast = document.getElementById("shod").value;
    window.location.href = "/Marahel/Parvaneh?shop=" + encodeURIComponent(shop) + "&shod=" + encodeURIComponent(sh_Darkhast) + "&codeMarhaleh=" + encodeURIComponent(codeMarhaleh);
}

// #endregion

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "ParvanehForm")
        ParvanehFormValidatoin(event);
});

function ParvanehFormValidatoin(e) {
    
    var valid = true;
    EnabledDate();

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده")) valid = false;

    var shod = document.getElementById("shod").value;
    if (!window.ValidationService.CheckRequired(shod, "شماره درخواست")) valid = false;
    if (!window.ValidationService.NotEqual(shod, 0, "شماره درخواست")) valid = false;

    var c_noeParvaneh = document.getElementById("c_noeParvaneh").value;
    if (!window.ValidationService.CheckRequired(c_noeParvaneh, "نوع پروانه")) valid = false;
    if (c_noeParvaneh != "" && !window.ValidationService.MoreThan(c_noeParvaneh, 0, "نوع پروانه")) valid = false;

    var noe_parvaneh = document.getElementById("noe_parvaneh").value;
    if (!window.ValidationService.isAlphanumeric(noe_parvaneh, "نوع پروانه")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noe_parvaneh, "نوع پروانه")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noe_parvaneh, 200, "نوع پروانه")) valid = false;

    var tarikh_parvaneh = document.getElementById("tarikh_parvaneh").value;
    if (!window.ValidationService.CheckRequired(tarikh_parvaneh, "تاریخ پروانه")) valid = false;
    if (tarikh_parvaneh != "" && !window.ValidationService.IsValidPersianDate(tarikh_parvaneh, "تاریخ پروانه")) valid = false;

    var tarikh_end_amaliat_s = document.getElementById("tarikh_end_amaliat_s").value;
    if (!window.ValidationService.CheckRequired(tarikh_end_amaliat_s, "تاریخ اتمام عملیات ساختمانی")) valid = false;
    if (tarikh_end_amaliat_s != "" && !window.ValidationService.IsValidPersianDate(tarikh_end_amaliat_s, "تاریخ اتمام عملیات ساختمانی")) valid = false;

    var tarikh_e_bimeh = document.getElementById("tarikh_e_bimeh").value;
    if (!window.ValidationService.CheckRequired(tarikh_e_bimeh, "تاریخ اعتبار بیمه نامه")) valid = false;
    if (tarikh_e_bimeh != "" && !window.ValidationService.IsValidPersianDate(tarikh_e_bimeh, "تاریخ اعتبار بیمه نامه")) valid = false;

    var sho_parvaneh = document.getElementById("sho_parvaneh").value;
    if (!window.ValidationService.CheckMaxLength(sho_parvaneh, 8, "شماره پروانه")) valid = false;
    
    var sho_bimenameh = document.getElementById("sho_bimenameh").value;
    if (!window.ValidationService.CheckLength(sho_bimenameh, 8, "شماره بیمه کارگران")) valid = false;

    var masahat_m_esh_zamin = document.getElementById("masahat_m_esh_zamin").value;
    if (!ValidationService.AmountIsValidFormat(masahat_m_esh_zamin, "مساحت زمین")) valid = false;
    if (!ValidationService.Between(masahat_m_esh_zamin, 0, 1000000000, "مساحت زمین")) valid = false;

    var masahat_m_s_tarakom = document.getElementById("masahat_m_s_tarakom").value;
    if (!ValidationService.AmountIsValidFormat(masahat_m_s_tarakom, "مساحت تراکم")) valid = false;
    if (!ValidationService.Between(masahat_m_s_tarakom, 0, 1000000000, "مساحت تراکم")) valid = false;

    var tozihat_parvaneh = document.getElementById("tozihat_parvaneh").value;
    if (!window.ValidationService.isAlphanumeric(tozihat_parvaneh, "توضیحات")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(tozihat_parvaneh, "توضیحات")) valid = false;
    if (tozihat_parvaneh != "" && !window.ValidationService.CheckMaxLength(tozihat_parvaneh, 500, "توضیحات")) valid = false;

    if (!valid)
        e.preventDefault();
}

// #endregion
