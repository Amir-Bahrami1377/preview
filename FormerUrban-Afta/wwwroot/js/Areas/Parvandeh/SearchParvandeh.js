// #region Business

document.addEventListener("change", function (event) {
    if (event.target.id === "SearchParvandehNoeSearch") {
        SearchParvandehNoeSearch(event);
    }
});

function SearchParvandehNoeSearch() {
    const label = document.getElementById("SearchParvandehLabel");
    document.getElementById("SearchParvandehInput").value = '';
    switch (document.getElementById("SearchParvandehNoeSearch").value) {
        case "1":
            label.innerHTML = "شماره پرونده";
            break;
        case "2":
            label.innerHTML = "کد نوسازی";
            break;
    }
}

// #endregion

// #region From Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "SearchParvandehForm") {
        return SearchParvandehValidation(event);
    }
});

function SearchParvandehValidation(e) {
    var isValid = true;
    var code = document.getElementById("SearchParvandehNoeSearch").value;
    var value = document.getElementById("SearchParvandehInput").value;

    if (code == 1 && !ValidationService.CheckRequired(value, "شماره پرونده"))
        isValid = false;

    else if (code == 1 && !ValidationService.CheckMaxLength(value, 20, "شماره پرونده"))
        isValid = false;

    else if (code == 1 && !ValidationService.IsDigitsOnly(value, "شماره پرونده"))
        isValid = false;

    if (code == 2 && !ValidationService.CheckRequired(value, "کد نوسازی"))
        isValid = false;

    else if (code == 2 && !ValidationService.CheckMaxLength(value, 50, "کد نوسازی"))
        isValid = false;

    else if (code == 2 && !ValidationService.IsValidCodeNosazi(value))
        isValid = false;

    if (!isValid)
        e.preventDefault();
}

// #endregion