document.addEventListener("keyup", function (event) {
    if (event.target.id === "ParametricSearch") {
        SearchParametric(event);
    }
});

function SearchParametric(event) {
    var value = event.target.value.toLowerCase();
    var rows = document.querySelectorAll("#ParametricTable tbody tr");

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
    if (event.target.closest('.ParametricRow')) {
        var row = event.target.closest('.ParametricRow');
        if (row) {
            var kodfarei = row.cells[0].textContent;
            var sharh = row.cells[1].textContent;
            var codeId = document.getElementById("ParametricCodeId").value;
            var titleId = document.getElementById("ParametricTitleId").value;
            document.getElementById(codeId).value = kodfarei;
            document.getElementById(titleId).value = sharh;
            CloseModal();
        }
    }
});