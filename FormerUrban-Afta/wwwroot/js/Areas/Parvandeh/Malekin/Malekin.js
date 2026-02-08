// #region Business
function CreateMalek(Id) {

    var id = Id ?? 0;
    var shop = document.getElementById("MalekinShop").value;
    var radif = document.getElementById("MalekinRadif").value;
    var NoeParvandeh = document.getElementById("MalekinNoeParvandeh").value;

    var model = new FormData();
    model.append("identity", id);
    model.append("shop", shop);
    model.append("radif", radif);
    model.append("NoeParvandeh", NoeParvandeh);

    var url = "Create";
    if (id > 0)
        url = "Edit";

    ShowModal(`/Parvandeh/Malekin/${url}`, model);
}

document.addEventListener("click", function (event) {
    if (event.target.id === "AddOrUpdateMalekin")
        MalekinFormValidatoin();

    if (event.target.id === "CreateMalek")
        CreateMalek();

    let item = event.target.closest(".MalekinEditRow");
    if (item && item.id) {
        var identity = document.getElementById(item.id).getAttribute("data-identity");
        CreateMalek(identity);
    }
});

// #endregion

// #region Validation

function MalekinFormValidatoin() {
    var valid = true;
    
    var Identity = document.getElementById("Identity").value;
    var id = document.getElementById("id").value;

    var shop = document.getElementById("shop").value;
    if (!window.ValidationService.CheckRequired(shop, "شماره پرونده مالک")) valid = false;
    if (!window.ValidationService.NotEqual(shop, 0, "شماره پرونده مالک")) valid = false;

    var d_radif = document.getElementById("d_radif").value;
    if (!window.ValidationService.CheckRequired(d_radif, "ردیف مالک")) valid = false;
    if (!window.ValidationService.NotEqual(d_radif, 0, "ردیف مالک")) valid = false;

    var mtable_name = document.getElementById("mtable_name").value;
    if (!window.ValidationService.CheckRequired(mtable_name, "نام جدول")) valid = false;
    if (mtable_name != "" && !window.ValidationService.NotEqual(mtable_name, "", "نام جدول")) valid = false;
    if (mtable_name != "" && !window.ValidationService.isAlphanumeric(mtable_name, "نام جدول")) valid = false;
    if (mtable_name != "" && !window.ValidationService.SanitizeAndValidateInput(mtable_name, "نام جدول")) valid = false;
    if (mtable_name != "" && !window.ValidationService.CheckMaxLength(mtable_name, 200, "نام جدول")) valid = false;


    var c_noemalek = document.getElementById("c_noemalek").value;
    if (!window.ValidationService.CheckRequired(c_noemalek, "نوع مالک")) valid = false;
    if (!ValidationService.Between(c_noemalek, 1, 2, "نوع مالک")) valid = false;
    
    var name = document.getElementById("name").value;
    if (!window.ValidationService.CheckRequired(name, "نام")) valid = false;
    if (name != "" && !window.ValidationService.IsPersianLetters(name, "نام")) valid = false;
    if (name != "" && !window.ValidationService.SanitizeAndValidateInput(name, "نام")) valid = false;
    if (name != "" && !window.ValidationService.CheckMaxLength(name, 200, "نام")) valid = false;

    var family = document.getElementById("family").value;
    if (!window.ValidationService.CheckRequired(family, "نام خانوادگی")) valid = false;
    if (family != "" && !window.ValidationService.IsPersianLetters(family, "نام خانوادگی")) valid = false;
    if (family != "" && !window.ValidationService.SanitizeAndValidateInput(family, "نام خانوادگی")) valid = false;
    if (family != "" && !window.ValidationService.CheckMaxLength(family, 200, "نام خانوادگی")) valid = false;

    var father = document.getElementById("father").value;
    if (father != "" && !window.ValidationService.IsPersianLetters(father, "نام پدر")) valid = false;
    if (father != "" && !window.ValidationService.SanitizeAndValidateInput(father, "نام پدر")) valid = false;
    if (father != "" && !window.ValidationService.CheckMaxLength(father, 200, "نام پدر")) valid = false;

    var kodemeli = document.getElementById("kodemeli").value;
    if (c_noemalek == 1) {
        if (!window.ValidationService.CheckRequired(kodemeli, "کد ملی")) valid = false;
        if (kodemeli != "" && !window.ValidationService.IsValidNationalCode(kodemeli)) valid = false;
    }
    if (c_noemalek == 2) {
        if (!window.ValidationService.CheckRequired(kodemeli, "شناسه ملی")) valid = false;
        if (kodemeli != "" && !window.ValidationService.IsValidLegalNationalId(kodemeli)) valid = false;
    }

    var sh_sh = document.getElementById("sh_sh").value;
    if (sh_sh != "" && !window.ValidationService.IsValidBirthCertificateNumber(sh_sh)) valid = false;

    var mob = document.getElementById("mob").value;
    if (!window.ValidationService.CheckRequired(mob, "شماره همراه")) valid = false;
    if (mob != "" && !ValidationService.IsValidMobileNumber(mob)) valid = false;

    var tel = document.getElementById("tel").value;
    if (tel != "" && !ValidationService.IsValidTel(tel)) valid = false;
    
    var sahm_a = document.getElementById("sahm_a").value;
    if (sahm_a != "" && !ValidationService.IsDigitsOnly(sahm_a,"سهم عرصه")) valid = false;
    if (sahm_a != "" && !ValidationService.Between(sahm_a, 0, 100, "سهم عرصه")) valid = false;

    var dong_a = document.getElementById("dong_a").value;
    if (dong_a != "" && !ValidationService.IsDigitsOnly(dong_a, "دانگ عرصه")) valid = false;
    if (dong_a != "" && !ValidationService.Between(dong_a, 0, 6, "دانگ عرصه")) valid = false;

    var sahm_b = document.getElementById("sahm_b").value;
    if (sahm_b != "" && !ValidationService.IsDigitsOnly(sahm_b, "سهم اعیان")) valid = false;
    if (sahm_b != "" && !ValidationService.Between(sahm_b, 0, 100, "سهم اعیان")) valid = false;

    var dong_b = document.getElementById("dong_b").value;
    if (dong_b != "" && !ValidationService.IsDigitsOnly(dong_b, "دانگ اعیان")) valid = false;
    if (dong_b != "" && !ValidationService.Between(dong_b, 0, 6, "دانگ اعیان")) valid = false;

    var ArzeshArse = document.getElementById("ArzeshArse").value;
    if (ArzeshArse != "" && !ValidationService.AmountIsValidFormat(ArzeshArse, "ارزش عرصه")) valid = false;
    if (ArzeshArse != "" && !ValidationService.Between(ArzeshArse, 0, 200000000000, "ارزش عرصه")) valid = false;

    var ArzeshAyan = document.getElementById("ArzeshAyan").value;
    if (ArzeshAyan != "" && !ValidationService.AmountIsValidFormat(ArzeshAyan, "ارزش اعیان")) valid = false;
    if (ArzeshAyan != "" && !ValidationService.Between(ArzeshAyan, 0, 200000000000, "ارزش اعیان")) valid = false;

    var address = document.getElementById("address").value;
    if (!window.ValidationService.CheckRequired(address, "آدرس")) valid = false;
    if (!window.ValidationService.isAlphanumeric(address, "آدرس")) valid = false;
    if (!window.ValidationService.SanitizeAndValidateInput(address, "آدرس")) valid = false;
    if (address != "" && !window.ValidationService.CheckMaxLength(address, 500, "آدرس")) valid = false;

    if (!valid)
        return false;

    let noemalek = c_noemalek == 1 ? 'حقیقی' : 'حقوقی';

    var model = new FormData();
    model.append("c_noemalek", c_noemalek);
    model.append("name", name);
    model.append("family", family);
    model.append("father", father);
    model.append("kodemeli", kodemeli);
    model.append("sh_sh", sh_sh);
    model.append("mob", mob);
    model.append("tel", tel);
    model.append("sahm_a", sahm_a);
    model.append("dong_a", dong_a);
    model.append("sahm_b", sahm_b);
    model.append("dong_b", dong_b);
    model.append("ArzeshArse", ArzeshArse);
    model.append("ArzeshAyan", ArzeshAyan);
    model.append("address", address);
    model.append("noemalek", noemalek);
    model.append("Identity", Identity);
    model.append("shop", shop);
    model.append("d_radif", d_radif);
    model.append("id", id);
    model.append("mtable_name", mtable_name);

    var url = "CreateSubmit";
    if (id > 0)
        url = "EditSubmit";

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: `/Parvandeh/Malekin/${url}`,
        data: model,
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.success)
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
