// #region Business

document.addEventListener("click", function (event) {
    if (event.target.id === "TaeedErsal")
        TaeedErsal();
    if (event.target.id === "TaeedErsalSubmit")
        TaeedErsalSubmit();
});

function TaeedErsal() {
    var shod = document.getElementById("shod").value;
    var codeMarhaleh = document.getElementById("codeMarhaleh").value;

    var model = new FormData();
    model.append("shod", shod);
    model.append("codeMarhaleh", codeMarhaleh);

    ShowModal('/Marahel/TaeedErsal/Index', model);
}

// #endregion

// #region Validation

function TaeedErsalSubmit() {
    var valid = true;
    
    var shop = document.getElementById("shop").value;
    var shod = document.getElementById("shod").value;
    var marhale = document.getElementById("marhale").value;
    var codeMarhale = document.getElementById("codeMarhale").value;

    if (shop == "" || shop <= 0) {
        valid = false;
        NotyfNews("خطا در پردازش شماره پرونده. لطفا مجددا وارد مرحله بشوید!!", "error");
    }

    if (shod == "" || shod <= 0) {
        valid = false;
        NotyfNews("خطا در پردازش شماره درخواست. لطفا مجددا وارد مرحله بشوید!!", "error");
    }

    if (codeMarhale == "" || codeMarhale <= 0) {
        valid = false;
        NotyfNews("خطا در پردازش مرحله بعدی. لطفا مجددا وارد مرحله بشوید!!", "error");
    }

    if (marhale == "") {
        valid = false;
        NotyfNews("خطا در پردازش مرحله جاری. لطفا مجددا وارد مرحله بشوید!!", "error");
    }
        

    if (!valid)
        e.preventDefault();

    var model = new FormData();
    model.append("shop",shop);
    model.append("shod",shod);
    model.append("marhale",marhale);
    model.append("codeMarhale",codeMarhale);

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: '/Marahel/TaeedErsal/SabtErsal',
        data: model,
        contentType: false,
        processData: false,
        success: function (data) {
            if (!data.success)
                NotyfNews(data.message, "error");
            else {
                CloseModal();
                window.location.href = "/Home/Index";
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
