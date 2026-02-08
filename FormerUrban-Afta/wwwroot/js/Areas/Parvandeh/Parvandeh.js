// #region Business Parvandeh

$(document).ready(function () {
    var shop = document.getElementById("FileInformationId").value;
    ShowParvandehInfo(shop);
});

document.addEventListener("click", function (event) {
    let item = event.target.closest(".ParvandehTreeView");
    let show = event.target.closest(".ShowParvandehDetail");
    if (item && item.id && show == null) {
        var selectedItem = SelectParvandehTree(item.id);
        if (selectedItem) {
            var shop = selectedItem.getAttribute("data-shop");
            ShowParvandehInfo(shop);
        }
    }
});

function ShowParvandehInfo(shop) {
    $.ajax({
        url: "/Parvandeh/ParvandehInfo/ParvandehInfo",
        method: "GET",
        data: { shop: shop },
        success: function (data) {
            $("#ParvandehInfo").html(data);
        },
        error: function (err) {
            if (err.status === 401)
                window.location.href = "/Login/Logout";
            else if (err.status === 429)
                window.location.href = "/Error/Error429"
            else
                NotyfNews(err.message, "error");
        }
    });
}

// #endregion

// #region Business Create Parvandeh

document.addEventListener("click", function (event) {
    if (event.target.id === "CreateParvandehBtnSubmit") {
        CreateParvandehSubmit();
    }
    if (event.target.id === "CreateMelk") {
        CreateParvandeh('2');
    }
    if (event.target.id === "CreateSakhteman") {
        CreateParvandeh('3');
    }
    if (event.target.id === "CreateAparteman") {
        CreateParvandeh('4');
    }
});

function CreateParvandeh(index) {
    let selectedItem = document.querySelector(".ParvandehTreeView.active");
    var shop = '';
    var codeN = '';
    if (selectedItem) {
        var idParent = selectedItem.getAttribute("data-idparent");
        shop = selectedItem.getAttribute("data-shop");
        codeN = selectedItem.getAttribute("data-coden");
        switch (index) {
            case '3':
                if (idParent != 0) {
                    NotyfNews('برای ایجاد ساختمان نیاز به انتخاب ملک است!', 'error');
                    return;
                }
                break;
            case '4':
                if (idParent != 1) {
                    NotyfNews('برای ایجاد آپارتمان نیاز به انتخاب ساختمان است!', 'error');
                    return;
                }
                break;
            case '5':
                if (idParent != 1 || idParent != 2) {
                    NotyfNews('برای ایجاد صنف نیاز به انتخاب ساختمان یا آپارتمان است!', 'error');
                    return;
                }
                break;
        }
    }
    else if (index > 2) {
        NotyfNews('لطفا یک پرونده را انتخاب کنید!', 'error');
        return;
    }

    var model = new FormData();
    model.append("index", index);
    model.append("shop", shop);
    model.append("codeN", codeN);

    ShowModal('/Parvandeh/ParvandehInfo/CreateParvandeh', model)
        .then(function (modal) {
            CreateParvandehEvent(index);
        })
        .catch(function (error) {
            NotyfNews("خطا در ایجاد پرونده", "error");
        });
}

function CreateParvandehEvent(index) {

    document.getElementById("CreateParvandehAparteman").readOnly = true;
    document.getElementById("CreateParvandehSakhteman").readOnly = true;
    document.getElementById("CreateParvandehMelk").readOnly = true;
    document.getElementById("CreateParvandehBlok").readOnly = true;
    document.getElementById("CreateParvandehMahaleh").readOnly = true;
    document.getElementById("CreateParvandehMantaghe").readOnly = true;

    switch (index) {
        case '2':
            document.getElementById("CreateParvandehMelk").readOnly = false;
            document.getElementById("CreateParvandehBlok").readOnly = false;
            document.getElementById("CreateParvandehMahaleh").readOnly = false;
            document.getElementById("CreateParvandehMantaghe").readOnly = false;
            break;
        case '3':
            document.getElementById("CreateParvandehSakhteman").readOnly = false;
            break;
        case '4':
            document.getElementById("CreateParvandehAparteman").readOnly = false;
            break;
    }
}

// #endregion

// #region Validation

function CreateParvandehSubmit() {
    var isValid = true;
    var index = document.getElementById("CreateParvandehIndex").value;
    var shop = document.getElementById("CreateParvandehParentShop").value;
    var mantaghe = document.getElementById("CreateParvandehMantaghe").value;
    var mahale = document.getElementById("CreateParvandehMahaleh").value;
    var blok = document.getElementById("CreateParvandehBlok").value;
    var melk = document.getElementById("CreateParvandehMelk").value;
    var sakhteman = document.getElementById("CreateParvandehSakhteman").value;
    var aparteman = document.getElementById("CreateParvandehAparteman").value;

    switch (index) {
        case "2":
            isValid = MelkValidation();
            break;
        case "3":
            isValid = SakhtemanValidation();
            break;
        case "4":
            isValid = ApartemanValidation();
            break;
        default:
            isValid = false;
    }

    if (!isValid)
        return;

    var model = new FormData();
    model.append("Index", index);
    model.append("shop", shop ?? "0");
    model.append("apar", aparteman);
    model.append("sakhteman", sakhteman);
    model.append("Melk", melk);
    model.append("blok", blok);
    model.append("hoze", mahale);
    model.append("mantaghe", mantaghe);

    $.ajax({
        cache: false,
        type: "POST",
        datatype: "JSON",
        url: '/Parvandeh/ParvandehInfo/Insert_Parvandeh',
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
                ParvandehView(data.shop);
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

function ParvandehView(shop) {
    
    var tokenElement = document.getElementById("RequestVerificationToken");
    var token = tokenElement ? tokenElement.value : "";

    var form = document.createElement("form");
    form.method = "POST";
    form.action = "/Parvandeh/ParvandehInfo/Index";

    var tokenInput = document.createElement("input");
    tokenInput.type = "hidden";
    tokenInput.name = "__RequestVerificationToken";
    tokenInput.value = token;
    form.appendChild(tokenInput);

    var shopInput = document.createElement("input");
    shopInput.type = "hidden";
    shopInput.name = "shop";
    shopInput.value = shop;
    form.appendChild(shopInput);

    document.body.appendChild(form);
    form.submit();
}

function MelkValidation() {
    var isValid = true;
    var index = document.getElementById("CreateParvandehIndex").value;
    var mantaghe = document.getElementById("CreateParvandehMantaghe").value;
    var mahale = document.getElementById("CreateParvandehMahaleh").value;
    var blok = document.getElementById("CreateParvandehBlok").value;
    var melk = document.getElementById("CreateParvandehMelk").value;
    var sakhteman = document.getElementById("CreateParvandehSakhteman").value;
    var aparteman = document.getElementById("CreateParvandehAparteman").value;

    if (index != "2") {
        NotyfNews("لطفا برای ایجاد ملک از بخش ایجاد ملک وارد شوید!", "error");
        isValid = false;
    }

    if (!ValidationService.MoreThan(mantaghe, 0, "منطقه"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(mantaghe, 7, "منطقه"))
        isValid = false;

    if (!ValidationService.MoreThan(mahale, 0, "محله"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(mahale, 7, "محله"))
        isValid = false;

    if (!ValidationService.MoreThan(blok, 0, "بلوک"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(blok, 7, "بلوک"))
        isValid = false;

    if (!ValidationService.MoreThan(melk, 0, "ملک"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(melk, 7, "ملک"))
        isValid = false;

    if (!ValidationService.Equal(sakhteman, 0, "ساختمان"))
        isValid = false;

    if (!ValidationService.Equal(aparteman, 0, "آپارتمان"))
        isValid = false;


    return isValid;
}

function SakhtemanValidation() {
    var isValid = true;
    var index = document.getElementById("CreateParvandehIndex").value;
    var mantaghe = document.getElementById("CreateParvandehMantaghe").value;
    var mahale = document.getElementById("CreateParvandehMahaleh").value;
    var blok = document.getElementById("CreateParvandehBlok").value;
    var melk = document.getElementById("CreateParvandehMelk").value;
    var sakhteman = document.getElementById("CreateParvandehSakhteman").value;
    var aparteman = document.getElementById("CreateParvandehAparteman").value;

    if (index != "3") {
        NotyfNews("لطفا برای ایجاد ساختمان از بخش ایجاد ساختمان وارد شوید!", "error");
        isValid = false;
    }

    if (!ValidationService.MoreThan(mantaghe, 0, "منطقه"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(mantaghe, 7, "منطقه"))
        isValid = false;

    if (!ValidationService.MoreThan(mahale, 0, "محله"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(mahale, 7, "محله"))
        isValid = false;

    if (!ValidationService.MoreThan(blok, 0, "بلوک"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(blok, 7, "بلوک"))
        isValid = false;

    if (!ValidationService.MoreThan(melk, 0, "ملک"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(melk, 7, "ملک"))
        isValid = false;

    if (!ValidationService.MoreThan(sakhteman, 0, "ساختمان"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(sakhteman, 7, "ساختمان"))
        isValid = false;

    if (!ValidationService.Equal(aparteman, 0, "آپارتمان"))
        isValid = false;

    return isValid;
}

function ApartemanValidation() {
    var isValid = true;
    var index = document.getElementById("CreateParvandehIndex").value;
    var mantaghe = document.getElementById("CreateParvandehMantaghe").value;
    var mahale = document.getElementById("CreateParvandehMahaleh").value;
    var blok = document.getElementById("CreateParvandehBlok").value;
    var melk = document.getElementById("CreateParvandehMelk").value;
    var sakhteman = document.getElementById("CreateParvandehSakhteman").value;
    var aparteman = document.getElementById("CreateParvandehAparteman").value;

    if (index != "4") {
        NotyfNews("لطفا برای ایجاد آپارتمان از بخش ایجاد آپارتمان وارد شوید!", "error");
        isValid = false;
    }

    if (!ValidationService.MoreThan(mantaghe, 0, "منطقه"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(mantaghe, 7, "منطقه"))
        isValid = false;

    if (!ValidationService.MoreThan(mahale, 0, "محله"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(mahale, 7, "محله"))
        isValid = false;

    if (!ValidationService.MoreThan(blok, 0, "بلوک"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(blok, 7, "بلوک"))
        isValid = false;

    if (!ValidationService.MoreThan(melk, 0, "ملک"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(melk, 7, "ملک"))
        isValid = false;

    if (!ValidationService.MoreThan(sakhteman, 0, "ساختمان"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(sakhteman, 7, "ساختمان"))
        isValid = false;

    if (!ValidationService.MoreThan(aparteman, 0, "آپارتمان"))
        isValid = false;
    if (!ValidationService.CheckMaxLength(aparteman, 7, "آپارتمان"))
        isValid = false;

    return isValid;
}

// #endregion
