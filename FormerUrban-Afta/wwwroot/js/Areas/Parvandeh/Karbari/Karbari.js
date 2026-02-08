// #region Business

function CreateKarbari(Id) {

    var id = Id ?? 0;
    var shop = document.getElementById("KarbariShop").value;
    var radif = document.getElementById("KarbariRadif").value;
    var NoeParvandeh = document.getElementById("KarbariNoeParvandeh").value;
    var codeMarhale = document.getElementById("KarbariCodeMarhale").value;

    var model = new FormData();
    model.append("identity", id);
    model.append("shop", shop);
    model.append("radif", radif);
    model.append("NoeParvandeh", NoeParvandeh);
    model.append("codeMarhale", codeMarhale);

    var url = "Create";
    if (id > 0)
        url = "Edit";

    ShowModal(`/Parvandeh/Dv_Karbari/${url}`, model);
}

document.addEventListener("click", function (event) {
    if (event.target.id === "AddOrUpdateKarbari")
        KarbariFormValidatoin();
    if (event.target.id === "CreateKarbari")
        CreateKarbari();

    let item = event.target.closest(".KarbariEditRow");
    if (item && item.id) {
        var identity = document.getElementById(item.id).getAttribute("data-identity");
        CreateKarbari(identity);
    }
});

// #endregion

// #region Validation

function KarbariFormValidatoin() {
    var valid = true;

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده کاربری")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده کاربری")) valid = false;

    var d_radif = document.getElementById("d_radif").value;
    if (!window.ValidationService.CheckRequired(d_radif, "ردیف کاربری")) valid = false;
    if (!window.ValidationService.NotEqual(d_radif, 0, "ردیف کاربری")) valid = false;

    var mtable_name = document.getElementById("mtable_name").value;
    if (!window.ValidationService.CheckRequired(mtable_name, "نام جدول")) valid = false;
    if (!window.ValidationService.isAlphanumeric(mtable_name, "نام جدول")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(mtable_name, "نام جدول")) valid = false;
    if (!window.ValidationService.CheckMaxLength(mtable_name, 200, "نام جدول")) valid = false;


    var c_tabagheh = document.getElementById("c_tabagheh").value;
    if (!window.ValidationService.CheckRequired(c_tabagheh, "طبقه")) valid = false;
    if (!window.ValidationService.MoreThan(c_tabagheh, 0, "طبقه")) valid = false;

    var tabagheh = $("#c_tabagheh option:selected").text();
    if (tabagheh != "" && !window.ValidationService.isAlphanumeric(tabagheh, "")) valid = false;
    if (tabagheh != "" && !window.ValidationService.SanitizeAndValidateInput(tabagheh, "")) valid = false;
    if (tabagheh != "" && !window.ValidationService.CheckMaxLength(tabagheh, 200, "")) valid = false;


    var c_karbari = document.getElementById("c_karbari").value;
    if (!window.ValidationService.CheckRequired(c_karbari, "کاربری")) valid = false;
    if (!window.ValidationService.MoreThan(c_karbari, 0, "کاربری")) valid = false;

    var karbari = $("#c_karbari option:selected").text();
    if (karbari != "" && !window.ValidationService.isAlphanumeric(karbari, "")) valid = false;
    if (karbari != "" && !window.ValidationService.SanitizeAndValidateInput(karbari, "")) valid = false;
    if (karbari != "" && !window.ValidationService.CheckMaxLength(karbari, 200, "")) valid = false;


    var c_noeestefadeh = document.getElementById("c_noeestefadeh").value;
    if (!window.ValidationService.CheckRequired(c_noeestefadeh, "نوع استفاده")) valid = false;
    if (!window.ValidationService.MoreThan(c_noeestefadeh, 0, "نوع استفاده")) valid = false;

    var noeestefadeh = $("#c_noeestefadeh option:selected").text();
    if (noeestefadeh != "" && !window.ValidationService.isAlphanumeric(noeestefadeh, "")) valid = false;
    if (noeestefadeh != "" && !window.ValidationService.SanitizeAndValidateInput(noeestefadeh, "")) valid = false;
    if (noeestefadeh != "" && !window.ValidationService.CheckMaxLength(noeestefadeh, 200, "")) valid = false;


    var c_noesakhteman = document.getElementById("c_noesakhteman").value;
    if (!window.ValidationService.CheckRequired(c_noesakhteman, "نوع ساختمان")) valid = false;
    if (!window.ValidationService.MoreThan(c_noesakhteman, 0, "نوع ساختمان")) valid = false;

    var noesakhteman = $("#c_noesakhteman option:selected").text();
    if (noesakhteman != "" && !window.ValidationService.isAlphanumeric(noesakhteman, "")) valid = false;
    if (noesakhteman != "" && !window.ValidationService.SanitizeAndValidateInput(noesakhteman, "")) valid = false;
    if (noesakhteman != "" && !window.ValidationService.CheckMaxLength(noesakhteman, 200, "")) valid = false;

    var c_noesazeh = document.getElementById("c_noesazeh").value;
    if (!window.ValidationService.CheckRequired(c_noesazeh, "نوع سازه")) valid = false;
    if (!window.ValidationService.MoreThan(c_noesazeh, 0, "نوع سازه")) valid = false;

    var noesazeh = $("#c_noesazeh option:selected").text();
    if (noesazeh != "" && !window.ValidationService.isAlphanumeric(noesazeh, "")) valid = false;
    if (noesazeh != "" && !window.ValidationService.SanitizeAndValidateInput(noesazeh, "")) valid = false;
    if (noesazeh != "" && !window.ValidationService.CheckMaxLength(noesazeh, 200, "")) valid = false;

    var c_marhaleh = document.getElementById("c_marhaleh").value;
    if (!window.ValidationService.CheckRequired(c_marhaleh, "مرحله")) valid = false;
    if (!window.ValidationService.MoreThan(c_marhaleh, 0, "مرحله")) valid = false;

    var marhaleh = $("#c_marhaleh option:selected").text();
    if (marhaleh != "" && !window.ValidationService.isAlphanumeric(marhaleh, "")) valid = false;
    if (marhaleh != "" && !window.ValidationService.SanitizeAndValidateInput(marhaleh, "")) valid = false;
    if (marhaleh != "" && !window.ValidationService.CheckMaxLength(marhaleh, 200, "")) valid = false;

    var tarikhehdas = document.getElementById("tarikhehdas").value;
    if (!window.ValidationService.CheckRequired(tarikhehdas, "تاریخ احداث")) valid = false;
    if (tarikhehdas != "" && !window.ValidationService.IsValidPersianDate(tarikhehdas, "تاریخ احداث")) valid = false;

    var masahat_k = document.getElementById("masahat_k").value;
    if (!window.ValidationService.CheckRequired(masahat_k, "مساحت کاربری")) valid = false;
    if (masahat_k != "" && !ValidationService.AmountIsValidFormat(masahat_k, "مساحت کاربری")) valid = false;
    if (masahat_k != "" && !ValidationService.Between(masahat_k, 0, 1000000000, "مساحت کاربری")) valid = false;

    if (!valid)
        return;

    var Identity = document.getElementById("Identity").value;
    var id = document.getElementById("id").value;
    var codeMarhale = document.getElementById("CodeMarhale").value;
    
    var model = new FormData();
    model.append("c_tabagheh", c_tabagheh);
    model.append("tabagheh", tabagheh);
    model.append("c_karbari", c_karbari);
    model.append("karbari", karbari);
    model.append("c_noeestefadeh", c_noeestefadeh);
    model.append("noeestefadeh", noeestefadeh);
    model.append("c_noesakhteman", c_noesakhteman);
    model.append("noesakhteman", noesakhteman);
    model.append("c_noesazeh", c_noesazeh);
    model.append("noesazeh", noesazeh);
    model.append("c_marhaleh", c_marhaleh);
    model.append("marhaleh", marhaleh);
    model.append("tarikhehdas", tarikhehdas);
    model.append("masahat_k", masahat_k);
    model.append("Identity", Identity);
    model.append("shop", shop);
    model.append("d_radif", d_radif);
    model.append("id", id);
    model.append("mtable_name", mtable_name);
    model.append("CodeMarhale", codeMarhale);

    var url = "CreateSubmit";
    if (id > 0)
        url = "EditSubmit";

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: `/Parvandeh/Dv_karbari/${url}`,
        data: model,
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.succecs)
                if (Array.isArray(data.message)) {
                    data.message.forEach(msg => NotyfNews(msg, "error"));
                } else {
                    NotyfNews(data.message, "error");
                }
            else {
                CloseModal();
                location.reload();
            }
        },
        error: function (data) {
            if (data.status === 429)
                window.location.href = "/Error/Error429";
            else
                NotyfNews(data.message, "error");
        }
    });
}

// #endregion

