
function ShowParametric(enumName, CodeId, TitleId) {
    var model = new FormData();
    model.append("enumName", enumName);
    model.append("CodeId", CodeId);
    model.append("TitleId", TitleId);
    ShowModal('/Sabetha/Index', model);
}


function ShowSabetha(enumName, CodeId, TitleId) {
    var model = new FormData();
    model.append("enumName", enumName);
    model.append("CodeId", CodeId);
    model.append("TitleId", TitleId);
    ShowModal('/Sabetha/Index', model);
}

document.addEventListener("keyup", function (event) {
    if (event.target.id === "SabethaSearch") {
        SearchSabetha(event);
    }
});

function SearchSabetha(event) {
    var value = event.target.value.toLowerCase();
    var rows = document.querySelectorAll("#SabethaTable tbody tr");

    rows.forEach(function (row) {
        var text = row.textContent || row.innerText;
        if (text && text.toLowerCase().indexOf(value) > -1) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}

document.addEventListener("dblclick", function (event) {
    if (event.target.closest('.SabethaRow')) {
        var row = event.target.closest('.SabethaRow');
        if (row) {
            var kodfarei = row.cells[0].textContent;
            var sharh = row.cells[1].textContent;
            var codeId = document.getElementById("SabethaCodeId").value;
            var titleId = document.getElementById("SabethaTitleId").value;
            document.getElementById(codeId).value = kodfarei;
            document.getElementById(titleId).value = sharh;
            CloseModal();
        }
    }
});
