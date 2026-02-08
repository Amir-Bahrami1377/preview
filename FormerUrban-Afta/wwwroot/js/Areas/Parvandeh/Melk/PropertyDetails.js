// #region Business

$(document).ready(function () {
    var errorElement = document.getElementById("PropertyDetailsErrorMessages");

    if (errorElement) { // بررسی وجود المنت
        var messageJson = errorElement.value;
        var messages = messageJson ? JSON.parse(messageJson) : [];

        if (messages.length > 0) {
            messages.forEach(msg => NotyfNews(msg, "error"));
            document.getElementById("MelkInfoEdit").classList.add("d-none");
        }
        else {
            $(".MelkInfoReadOnly").prop("disabled", true);
            $(".MelkInfoReadOnly").prop("readonly", true);

            var cancel = document.getElementById("MelkInfoCancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("MelkInfoSubmit");
            if (submit)
                submit.classList.add("d-none");
        }
    }

});

document.addEventListener("click", function (event) {
    if (event.target.id === "MelkInfoEdit") {
        EditMelk(event);
    }
    if (event.target.id === "MelkInfoCancel") {
        CancelMelk(event);
    }
    if (event.target.id === "MelkInfo-noesanadBtn") {
        ShowParametric('EnumNoeSanad', 'MelkInfo-c_noesanad', 'MelkInfo-noesanad');
    }
    if (event.target.id === "MelkInfo-vazsanadBtn") {
        ShowParametric('EnumVazSanad', 'MelkInfo-c_vazsanad', 'MelkInfo-vazsanad');
    }
    if (event.target.id === "MelkInfo-vazmelkBtn") {
        ShowParametric('EnumVazMelk', 'MelkInfo-c_vazmelk', 'MelkInfo-vazmelk');
    }
    if (event.target.id === "MelkInfo-noemelkBtn") {
        ShowParametric('EnumNoeMelk', 'MelkInfo-c_noemelk', 'MelkInfo-noemelk');
    }
    if (event.target.id === "MelkInfo-marhalehBtn") {
        ShowSabetha('EnumMarhalehMelkType', 'MelkInfo-c_marhaleh', 'MelkInfo-marhaleh');
    }
    if (event.target.id === "MelkInfo-mahdodehBtn") {
        ShowParametric('EnumMahdudehType', 'MelkInfo-c_mahdodeh', 'MelkInfo-mahdodeh');
    }
    if (event.target.id === "MelkInfo-KarbariAsliBtn") {
        ShowSabetha('EnumKarbariType', 'MelkInfo-C_karbariAsli', 'MelkInfo-KarbariAsli');
    }
});

function EditMelk() {
    document.getElementById("MelkInfoCancel").classList.remove("d-none");
    document.getElementById("MelkInfoSubmit").classList.remove("d-none");
    document.getElementById("MelkInfoEdit").classList.add("d-none");
    $(".MelkInfoReadOnly").prop("disabled", false);
    $(".MelkInfoReadOnly").prop("readonly", false);
}

function CancelMelk() {
    document.getElementById("PropertyDetailsErrorMessages").value = "";
    var shop = document.getElementById("shop").value;
    var sh_Darkhast = document.getElementById("sh_Darkhast").value;
    var dShop = document.getElementById("dShop").value;

    var tokenElement = document.getElementById("RequestVerificationToken");
    var token = tokenElement ? tokenElement.value : "";

    var form = document.createElement("form");
    form.method = "POST";
    form.action = "/Parvandeh/Melk/PropertyDetails";

    var tokenInput = document.createElement("input");
    tokenInput.type = "hidden";
    tokenInput.name = "__RequestVerificationToken";
    tokenInput.value = token;
    form.appendChild(tokenInput);

    var inputs = [
        { name: "shop", value: shop },
        { name: "shod", value: sh_Darkhast },
        { name: "dShop", value: dShop }
    ];

    inputs.forEach(function (item) {
        var input = document.createElement("input");
        input.type = "hidden";
        input.name = item.name;
        input.value = item.value;
        form.appendChild(input);
    });

    document.body.appendChild(form);
    form.submit();
}


// #endregion

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "MelkInfoForm")
        MelkFormValidation(event);
});

function MelkFormValidation(event) {
    var valid = true;

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده ملک")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده ملک")) valid = false;

    var radif = document.getElementById("radif").value;
    if (!window.ValidationService.CheckRequired(radif, "ردیف ملک")) valid = false;
    if (!window.ValidationService.NotEqual(radif, 0, "ردیف ملک")) valid = false;

    var Active = document.getElementById("Active").value;
    if (!window.ValidationService.CheckRequired(Active, "وضعیت پرونده")) valid = false;


    var Cnoesanad = document.getElementById("MelkInfo-c_noesanad").value;
    if (!window.ValidationService.MoreThan(Cnoesanad, 0, "کد نوع سند")) valid = false;
    
    var noesanad = document.getElementById("MelkInfo-noesanad").value;
    if (!window.ValidationService.isAlphanumeric(noesanad, "نوع سند")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noesanad, "نوع سند")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noesanad, 200, "نوع سند")) valid = false;
    if (Cnoesanad == "" && noesanad != "") valid = false;

    var Cvazsanad = document.getElementById("MelkInfo-c_vazsanad").value;
    if (!window.ValidationService.MoreThan(Cvazsanad, 0, "کد وضعیت سند")) valid = false;

    var vazsanad = document.getElementById("MelkInfo-vazsanad").value;
    if (!window.ValidationService.isAlphanumeric(vazsanad, "وضعیت سند")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(vazsanad, "وضعیت سند")) valid = false;
    if (!window.ValidationService.CheckMaxLength(vazsanad, 200, "وضعیت سند")) valid = false;
    if (Cvazsanad == "" && vazsanad != "") valid = false;


    var Cvazmelk = document.getElementById("MelkInfo-c_vazmelk").value;
    if (!window.ValidationService.MoreThan(Cvazmelk, 0, "کد وضعیت ملک")) valid = false;

    var vazmelk = document.getElementById("MelkInfo-vazmelk").value;
    if (!window.ValidationService.isAlphanumeric(vazmelk, "وضعیت ملک")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(vazmelk, "وضعیت ملک")) valid = false;
    if (!window.ValidationService.CheckMaxLength(vazmelk, 200, "وضعیت ملک")) valid = false;
    if (Cvazmelk == "" && vazmelk != "") valid = false;


    var Cnoemelk = document.getElementById("MelkInfo-c_noemelk").value;
    if (!window.ValidationService.MoreThan(Cnoemelk, 0, "کد نوع ملک")) valid = false;

    var noemelk = document.getElementById("MelkInfo-noemelk").value;
    if (!window.ValidationService.isAlphanumeric(noemelk, "نوع ملک")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noemelk, "نوع ملک")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noemelk, 200, "نوع ملک")) valid = false;
    if (Cnoemelk == "" && noemelk != "") valid = false;


    var Cmarhaleh = document.getElementById("MelkInfo-c_marhaleh").value;
    if (!window.ValidationService.MoreThan(Cmarhaleh, 0, "کد مرحله ساخت")) valid = false;

    var marhaleh = document.getElementById("MelkInfo-marhaleh").value;
    if (!window.ValidationService.isAlphanumeric(marhaleh, "مرحله ساخت")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(marhaleh, "مرحله ساخت")) valid = false;
    if (!window.ValidationService.CheckMaxLength(marhaleh, 200, "مرحله ساخت")) valid = false;
    if (Cmarhaleh == "" && marhaleh != "") valid = false;


    var Cmahdodeh = document.getElementById("MelkInfo-c_mahdodeh").value;
    if (!window.ValidationService.MoreThan(Cmahdodeh, 0, "کد محدوده")) valid = false;

    var mahdodeh = document.getElementById("MelkInfo-mahdodeh").value;
    if (!window.ValidationService.isAlphanumeric(mahdodeh, "محدوده")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(mahdodeh, "محدوده")) valid = false;
    if (!window.ValidationService.CheckMaxLength(mahdodeh, 200, "محدوده")) valid = false;
    if (Cmahdodeh == "" && mahdodeh != "") valid = false;


    var CKarbariAsli = document.getElementById("MelkInfo-C_karbariAsli").value;
    if (!window.ValidationService.MoreThan(CKarbariAsli, 0, "کد کاربری اصلی")) valid = false;

    var KarbariAsli = document.getElementById("MelkInfo-KarbariAsli").value;
    if (!window.ValidationService.isAlphanumeric(KarbariAsli, "کاربری اصلی")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(KarbariAsli, "کاربری اصلی")) valid = false;
    if (!window.ValidationService.CheckMaxLength(KarbariAsli, 200, "کاربری اصلی")) valid = false;
    if (CKarbariAsli == "" && KarbariAsli != "") valid = false;

    
    var pelakabi = document.getElementById("MelkInfo-pelakabi").value;
    if (!ValidationService.Between(pelakabi, 0, 999999, "پلاک آبی"))
        valid = false;

    var tel = document.getElementById("MelkInfo-tel").value;
    if (tel != "" && !ValidationService.IsValidTel(tel)) valid = false;

    var codeposti = document.getElementById("MelkInfo-codeposti").value;
    if (codeposti != "" && !ValidationService.IsValidPostalCode(codeposti)) valid = false;

    var ArzeshArse = document.getElementById("MelkInfo-ArzeshArse").value;
    if (!ValidationService.AmountIsValidFormat(ArzeshArse, "ارزش عرصه")) valid = false;
    if (!ValidationService.Between(ArzeshArse, 0, 200000000000, "ارزش عرصه")) valid = false;

    var masahat_s = document.getElementById("MelkInfo-masahat_s").value;
    if (!ValidationService.AmountIsValidFormat(masahat_s, "مساحت طبق سند")) valid = false;
    if (!ValidationService.Between(masahat_s, 0, 1000000000, "مساحت طبق سند")) valid = false;

    var masahat_m = document.getElementById("MelkInfo-masahat_m").value;
    if (!ValidationService.AmountIsValidFormat(masahat_m, "مساحت موجود")) valid = false;
    if (!ValidationService.Between(masahat_m, 0, 1000000000, "مساحت موجود")) valid = false;

    var masahat_e = document.getElementById("MelkInfo-masahat_e").value;
    if (!ValidationService.AmountIsValidFormat(masahat_e, "مساحت اصلاحی")) valid = false;
    if (!ValidationService.Between(masahat_e, 0, 1000000000, "مساحت اصلاحی")) valid = false;

    var masahat_b = document.getElementById("MelkInfo-masahat_b").value;
    if (!ValidationService.AmountIsValidFormat(masahat_b, "مساحت باقیمانده")) valid = false;
    if (!ValidationService.Between(masahat_b, 0, 1000000000, "مساحت باقیمانده")) valid = false;

    var utmx = document.getElementById("utmx").value;
    if (utmx != 0 && !ValidationService.IsValidUtmX(utmx)) valid = false;

    var utmy = document.getElementById("utmy").value;
    if (utmy != 0 && !ValidationService.IsValidUtmY(utmy)) valid = false;

    var tafkiki = document.getElementById("MelkInfo-tafkiki").value;
    if (!ValidationService.Between(tafkiki, 0, 999, "قطعه تفکیکی")) valid = false;

    var fari = document.getElementById("MelkInfo-fari").value;
    if (!ValidationService.Between(fari, 0, 99999, "فرعی")) valid = false;

    var azFari = document.getElementById("MelkInfo-azFari").value;
    if (!ValidationService.Between(azFari, 0, 99999, "از فرعی")) valid = false;

    var asli = document.getElementById("MelkInfo-asli").value;
    if (!ValidationService.Between(asli, 0, 99999, "اصلی")) valid = false;

    var bakhsh = document.getElementById("MelkInfo-bakhsh").value;
    if (!ValidationService.Between(bakhsh, 0, 99, "بخش")) valid = false;

    var address = document.getElementById("MelkInfo-address").value;
    if (!window.ValidationService.isAlphanumeric(address, "آدرس")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(address, "آدرس")) valid = false;
    if (!window.ValidationService.CheckMaxLength(address, 500, "آدرس")) valid = false;


    if (!valid)
        event.preventDefault();
}
// #endregion