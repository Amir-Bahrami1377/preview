// #region Business

$(document).ready(function () {
    var errorElement = document.getElementById("ApartemanErrorMessages");

    if (errorElement) { // بررسی وجود المنت
        var messageJson = errorElement.value;
        var messages = messageJson ? JSON.parse(messageJson) : [];

        if (messages.length > 0) {
            messages.forEach(msg => NotyfNews(msg, "error"));
            document.getElementById("ApartemanInfoEdit").classList.add("d-none");
        }
        else {
            $(".ApartemanInfoReadOnly").prop("disabled", true);
            $(".ApartemanInfoReadOnly").prop("readonly", true);

            var cancel = document.getElementById("ApartemanInfoCancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("ApartemanInfoSubmit");
            if (submit)
                submit.classList.add("d-none");
        }
    }

});

document.addEventListener("click", function (event) {
    if (event.target.id === "ApartemanInfoEdit") {
        EditAparteman(event);
    }
    if (event.target.id === "ApartemanInfoCancel") {
        CancelAparteman(event);
    }
    if (event.target.id === "ApartemanInfoNoesanadBtn") {
        ShowParametric('EnumNoeSanad', 'c_noesanad', 'noesanad');
    }
    if (event.target.id === "ApartemanInfoVazsanadBtn") {
        ShowParametric('EnumVazSanad', 'c_vazsanad', 'vazsanad');
    }
    if (event.target.id === "ApartemanInfoNoemalekiyatBtn") {
        ShowParametric('EnumNoemalekiat', 'c_noemalekiyat', 'noemalekiyat');
    }
    if (event.target.id === "ApartemanInfoNoeSazeBtn") {
        ShowParametric('EnumNoeSaze', 'c_NoeSaze', 'NoeSaze');
    }
    if (event.target.id === "ApartemanInfoJahatBtn") {
        ShowParametric('EnumJahatType', 'C_Jahat', 'jahat');
    }
});

function EditAparteman() {
    document.getElementById("ApartemanInfoCancel").classList.remove("d-none");
    document.getElementById("ApartemanInfoSubmit").classList.remove("d-none");
    document.getElementById("ApartemanInfoEdit").classList.add("d-none");
    $(".ApartemanInfoReadOnly").prop("disabled", false);
    $(".ApartemanInfoReadOnly").prop("readonly", false);
}

function CancelAparteman() {
    document.getElementById("ApartemanErrorMessages").value = "";
    var shop = document.getElementById("ApartemanInfoShop").value;
    var sh_Darkhast = document.getElementById("shod").value;
    var dShop = document.getElementById("dShop").value;

    var tokenElement = document.getElementById("RequestVerificationToken");
    var token = tokenElement ? tokenElement.value : "";

    var form = document.createElement("form");
    form.method = "POST";
    form.action = "/Parvandeh/Apartman/Index";

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
    if (event.target.id === "ApartemanInfoForm")
        ApartemanFormValidatoin(event);
});

function ApartemanFormValidatoin(event) {
    var valid = true;
    
    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده آپارتمان")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده آپارتمان")) valid = false;

    var radif = document.getElementById("radif").value;
    if (!window.ValidationService.CheckRequired(radif, "ردیف آپارتمان")) valid = false;
    if (!window.ValidationService.NotEqual(radif, 0, "ردیف آپارتمان")) valid = false;

    var Active = document.getElementById("Active").value;
    if (!window.ValidationService.CheckRequired(Active, "وضعیت پرونده")) valid = false;


    var c_noesanad = document.getElementById("c_noesanad").value;
    if (!window.ValidationService.CheckRequired(c_noesanad, "نوع سند")) valid = false;
    if (c_noesanad != "" && !window.ValidationService.MoreThan(c_noesanad, 0, "نوع سند")) valid = false;

    var noesanad = document.getElementById("noesanad").value;
    if (!window.ValidationService.isAlphanumeric(noesanad, "نوع سند")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noesanad, "نوع سند")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noesanad, 200, "نوع سند")) valid = false;

    var c_vazsanad = document.getElementById("c_vazsanad").value;
    if (!window.ValidationService.CheckRequired(c_vazsanad, "وضعیت سند")) valid = false;
    if (c_vazsanad != "" && !window.ValidationService.MoreThan(c_vazsanad, 0, "وضعیت سند")) valid = false;

    var vazsanad = document.getElementById("vazsanad").value;
    if (!window.ValidationService.isAlphanumeric(vazsanad, "وضعیت سند")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(vazsanad, "وضعیت سند")) valid = false;
    if (!window.ValidationService.CheckMaxLength(vazsanad, 200, "وضعیت سند")) valid = false;

    var c_noemalekiyat = document.getElementById("c_noemalekiyat").value;
    if (!window.ValidationService.CheckRequired(c_noemalekiyat, "نوع مالکیت")) valid = false;
    if (c_noemalekiyat != "" && !window.ValidationService.MoreThan(c_noemalekiyat, 0, "نوع مالکیت")) valid = false;

    var noemalekiyat = document.getElementById("noemalekiyat").value;
    if (!window.ValidationService.isAlphanumeric(noemalekiyat, "نوع مالکیت")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(noemalekiyat, "نوع مالکیت")) valid = false;
    if (!window.ValidationService.CheckMaxLength(noemalekiyat, 200, "نوع مالکیت")) valid = false;

    var c_NoeSaze = document.getElementById("c_NoeSaze").value;
    if (!window.ValidationService.CheckRequired(c_NoeSaze, "نوع سازه")) valid = false;
    if (c_NoeSaze != "" && !window.ValidationService.MoreThan(c_NoeSaze, 0, "نوع سازه")) valid = false;

    var NoeSaze = document.getElementById("NoeSaze").value;
    if (!window.ValidationService.isAlphanumeric(NoeSaze, "نوع سازه")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(NoeSaze, "نوع سازه")) valid = false;
    if (!window.ValidationService.CheckMaxLength(NoeSaze, 200, "نوع سازه")) valid = false;

    var C_Jahat = document.getElementById("C_Jahat").value;
    if (!window.ValidationService.CheckRequired(C_Jahat, "جهت")) valid = false;
    if (C_Jahat != "" && !window.ValidationService.MoreThan(C_Jahat, 0, "جهت")) valid = false;

    var jahat = document.getElementById("jahat").value;
    if (!window.ValidationService.isAlphanumeric(jahat, "جهت")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(jahat, "جهت")) valid = false;
    if (!window.ValidationService.CheckMaxLength(jahat, 200, "جهت")) valid = false;

    var pelakabi = document.getElementById("pelakabi").value;
    if (pelakabi != "" && !ValidationService.Between(pelakabi, 0, 999999, "پلاک آبی")) valid = false;

    var tel = document.getElementById("tel").value;
    if (!window.ValidationService.CheckRequired(tel, "تلفن")) valid = false;
    if (tel != "" && !ValidationService.IsValidTel(tel)) valid = false;

    var codeposti = document.getElementById("codeposti").value;
    if (!window.ValidationService.CheckRequired(codeposti, "کد پستی")) valid = false;
    if (codeposti != "" && !ValidationService.IsValidPostalCode(codeposti)) valid = false;

    var MasahatKol = document.getElementById("MasahatKol").value;
    if (!window.ValidationService.CheckRequired(MasahatKol, "مساحت کل")) valid = false;
    if (MasahatKol != "" && !ValidationService.AmountIsValidFormat(MasahatKol, "مساحت کل")) valid = false;
    if (MasahatKol != "" && !ValidationService.Between(MasahatKol, 0, 1000000000, "مساحت کل")) valid = false;

    var MasahatArse = document.getElementById("MasahatArse").value;
    if (!window.ValidationService.CheckRequired(MasahatArse, "مساحت عرصه")) valid = false;
    if (MasahatArse != "" && !ValidationService.AmountIsValidFormat(MasahatArse, "مساحت عرصه")) valid = false;
    if (MasahatArse != "" && !ValidationService.Between(MasahatArse, 0, 1000000000, "مساحت عرصه")) valid = false;

    var tafkiki = document.getElementById("tafkiki").value;
    if (!ValidationService.Between(tafkiki, 0, 999, "قطعه تفکیکی")) valid = false;

    var fari = document.getElementById("fari").value;
    if (!ValidationService.Between(fari, 0, 99999, "فرعی")) valid = false;

    var azFari = document.getElementById("azFari").value;
    if (!ValidationService.Between(azFari, 0, 99999, "از فرعی")) valid = false;

    var asli = document.getElementById("asli").value;
    if (!ValidationService.Between(asli, 0, 99999, "اصلی")) valid = false;

    var bakhsh = document.getElementById("bakhsh").value;
    if (!ValidationService.Between(bakhsh, 0, 99, "بخش")) valid = false;

    var address = document.getElementById("address").value;
    if (!window.ValidationService.CheckRequired(address, "آدرس")) valid = false;
    if (!window.ValidationService.isAlphanumeric(address, "آدرس")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(address, "آدرس")) valid = false;
    if (!window.ValidationService.CheckMaxLength(address, 500, "آدرس")) valid = false;

    if (!valid)
        event.preventDefault();
}

// #endregion