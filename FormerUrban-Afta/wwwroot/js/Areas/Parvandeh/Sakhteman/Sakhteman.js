// #region Business

$(document).ready(function () {
    var errorElement = document.getElementById("SakhtemanErrorMessages");

    if (errorElement) { // بررسی وجود المنت
        var messageJson = errorElement.value;
        var messages = messageJson ? JSON.parse(messageJson) : [];

        if (messages.length > 0) {
            messages.forEach(msg => NotyfNews(msg, "error"));
            document.getElementById("SakhtemanInfoEdit").classList.add("d-none");
        }
        else {
            $(".SakhtemanInfoReadOnly").prop("disabled", true);
            $(".SakhtemanInfoReadOnly").prop("readonly", true);

            var cancel = document.getElementById("SakhtemanInfoCancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("SakhtemanInfoSubmit");
            if (submit)
                submit.classList.add("d-none");
        }
    }
    ShowDate();
    DisabledDate();
});

document.addEventListener("click", function (event) {
    if (event.target.id === "SakhtemanInfoEdit") {
        EditSakhteman(event);
    }
    if (event.target.id === "SakhtemanInfoCancel") {
        CancelSakhteman(event);
    }
    if (event.target.id === "SakhtemanInfoNoenamaBtn") {
        ShowParametric('EnumNoename', 'c_noenama', 'noenama');
    }
    if (event.target.id === "SakhtemanInfoNoesaghfBtn") {
        ShowParametric('EnumNoeSaghf', 'c_noesaghf', 'noesaghf');
    }
    if (event.target.id === "SakhtemanInfoMarhalehBtn") {
        ShowParametric('EnumMarhalehSakht', 'c_marhaleh', 'marhaleh');
    }
    if (event.target.id === "SakhtemanInfoNoeSakhtemanBtn") {
        ShowParametric('EnumNoeSakhteman', 'c_NoeSakhteman', 'NoeSakhteman');
    }
    if (event.target.id === "SakhtemanInfoNoeSazeBtn") {
        ShowParametric('EnumNoeSaze', 'c_NoeSaze', 'NoeSaze');
    }
});

function EditSakhteman() {
    document.getElementById("SakhtemanInfoCancel").classList.remove("d-none");
    document.getElementById("SakhtemanInfoSubmit").classList.remove("d-none");
    document.getElementById("SakhtemanInfoEdit").classList.add("d-none");
    $(".SakhtemanInfoReadOnly").prop("disabled", false);
    $(".SakhtemanInfoReadOnly").prop("readonly", false);
    EnabledDate();
}

function CancelSakhteman() {
    document.getElementById("SakhtemanErrorMessages").value = "";
    var shop = document.getElementById("SakhtemanInfoShop").value;
    var dShop = document.getElementById("dShop").value;
    var sh_Darkhast = document.getElementById("shod").value;
    window.location.href = "/Parvandeh/Sakhteman?strShoP=" + encodeURIComponent(shop)
        + "&shod=" + encodeURIComponent(sh_Darkhast) + "&dShop=" + encodeURIComponent(dShop);
}

function CancelSakhteman() {
    document.getElementById("SakhtemanErrorMessages").value = "";
    var shop = document.getElementById("SakhtemanInfoShop").value;
    var sh_Darkhast = document.getElementById("shod").value;
    var dShop = document.getElementById("dShop").value;

    var tokenElement = document.getElementById("RequestVerificationToken");
    var token = tokenElement ? tokenElement.value : "";

    var form = document.createElement("form");
    form.method = "POST";
    form.action = "/Parvandeh/Sakhteman/Index";

    var tokenInput = document.createElement("input");
    tokenInput.type = "hidden";
    tokenInput.name = "__RequestVerificationToken";
    tokenInput.value = token;
    form.appendChild(tokenInput);

    var inputs = [
        { name: "strShoP", value: shop },
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
    if (event.target.id === "SakhtemanInfoForm")
        SakhtemanFormValidatoin(event);
});

function SakhtemanFormValidatoin(e) {
    EnabledDate();
    var valid = true;
    
    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده ساختمان")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده ساختمان")) valid = false;

    var radif = document.getElementById("radif").value;
    if (!window.ValidationService.CheckRequired(radif, "ردیف ساختمان")) valid = false;
    if (!window.ValidationService.NotEqual(radif, 0, "ردیف ساختمان")) valid = false;

    var Active = document.getElementById("Active").value;
    if (!window.ValidationService.CheckRequired(Active, "وضعیت پرونده")) valid = false;


    var c_noenama = document.getElementById("c_noenama").value;
    if (!window.ValidationService.CheckRequired(c_noenama, "نوع نما")) valid = false;
    if (c_noenama != "" && !window.ValidationService.MoreThan(c_noenama, 0, "نوع نما")) valid = false;

    var noenama = document.getElementById("noenama").value;
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


    var c_marhaleh = document.getElementById("c_marhaleh").value;
    if (!window.ValidationService.CheckRequired(c_marhaleh, "مرحله ساختمانی")) valid = false;
    if (c_marhaleh != "" && !window.ValidationService.MoreThan(c_marhaleh, 0, "مرحله ساختمانی")) valid = false;

    var marhaleh = document.getElementById("marhaleh").value;
    if (!window.ValidationService.isAlphanumeric(marhaleh, "مرحله ساختمانی")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(marhaleh, "مرحله ساختمانی")) valid = false;
    if (!window.ValidationService.CheckMaxLength(marhaleh, 200, "مرحله ساختمانی")) valid = false;


    var c_NoeSakhteman = document.getElementById("c_NoeSakhteman").value;
    if (!window.ValidationService.CheckRequired(c_NoeSakhteman, "نوع ساختمان")) valid = false;
    if (c_NoeSakhteman != "" && !window.ValidationService.MoreThan(c_NoeSakhteman, 0, "نوع ساختمان")) valid = false;

    var NoeSakhteman = document.getElementById("NoeSakhteman").value;
    if (!window.ValidationService.isAlphanumeric(NoeSakhteman, "نوع ساختمان")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(NoeSakhteman, "نوع ساختمان")) valid = false;
    if (!window.ValidationService.CheckMaxLength(NoeSakhteman, 200, "نوع ساختمان")) valid = false;


    var c_NoeSaze = document.getElementById("c_NoeSaze").value;
    if (!window.ValidationService.CheckRequired(c_NoeSaze, "نوع سازه")) valid = false;
    if (c_NoeSaze != "" && !window.ValidationService.MoreThan(c_NoeSaze, 0, "نوع سازه")) valid = false;

    var NoeSaze = document.getElementById("NoeSaze").value;
    if (!window.ValidationService.isAlphanumeric(NoeSaze, "نوع سازه")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(NoeSaze, "نوع سازه")) valid = false;
    if (!window.ValidationService.CheckMaxLength(NoeSaze, 200, "نوع سازه")) valid = false;


    var TarikhEhdas = document.getElementById("TarikhEhdas").value;
    if (TarikhEhdas != "" && !window.ValidationService.IsValidPersianDate(TarikhEhdas, "تاریخ احداث")) valid = false;

    var masahatkol = document.getElementById("masahatkol").value;
    if (masahatkol != "" && !ValidationService.AmountIsValidFormat(masahatkol, "مساحت کل زیربنا")) valid = false;
    if (masahatkol != "" && !ValidationService.Between(masahatkol, 0, 1000000000, "مساحت کل زیربنا")) valid = false;

    var MasahatZirbana = document.getElementById("MasahatZirbana").value;
    if (MasahatZirbana != "" && !ValidationService.AmountIsValidFormat(MasahatZirbana, "مساحت زیربنا")) valid = false;
    if (MasahatZirbana != "" && !ValidationService.Between(MasahatZirbana, 0, 1000000000, "مساحت زیربنا")) valid = false;

    var MasahatArse = document.getElementById("MasahatArse").value;
    if (MasahatArse != "" && !ValidationService.AmountIsValidFormat(MasahatArse, "مساحت سهم العرصه")) valid = false;
    if (MasahatArse != "" && !ValidationService.Between(MasahatArse, 0, 1000000000, "مساحت سهم العرصه")) valid = false;

    var tarakom = document.getElementById("tarakom").value;
    if (tarakom != "" && !ValidationService.AmountIsValidFormat(tarakom, "مساحت مجاز در پروانه")) valid = false;
    if (tarakom != "" && !ValidationService.Between(tarakom, 0, 1000000000, "مساحت مجاز در پروانه")) valid = false;

    var satheshghal = document.getElementById("satheshghal").value;
    if (satheshghal != "" && !ValidationService.AmountIsValidFormat(satheshghal, "سطح اشغال")) valid = false;
    if (satheshghal != "" && !ValidationService.Between(satheshghal, 0, 1000000000, "سطح اشغال")) valid = false;

    var ArzeshAyan = document.getElementById("ArzeshAyan").value;
    if (ArzeshAyan != "" && !ValidationService.AmountIsValidFormat(ArzeshAyan, "ارزش اعیان")) valid = false;
    if (ArzeshAyan != "" && !ValidationService.Between(ArzeshAyan, 0, 200000000000, "ارزش اعیان")) valid = false;

    var TedadTabaghe = document.getElementById("TedadTabaghe").value;
    if (TedadTabaghe != "" && !ValidationService.Between(TedadTabaghe, 0, 99, "تعداد طبقه")) valid = false;

    if (!valid)
        e.preventDefault();
}

// #endregion
