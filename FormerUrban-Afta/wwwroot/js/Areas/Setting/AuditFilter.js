document.addEventListener("click", function (event) {
    if (event.target.id === "CreateAuditFilters")
        ShowModal("/Setting/AuditFilter/Create", "");

    if (event.target.id === "CreateAuditFiltersForm")
        CreateAuditFiltersFormValidatoin("");

    let item = event.target.closest(".AuditFiltersDeleteRow");
    if (item && item.id) {
        var identity = document.getElementById(item.id).getAttribute("data-identity");
        DeleteAuditFilter(identity);
    }
});

function CreateAuditFiltersFormValidatoin() {
    var valid = true;
    var FormName = document.getElementById('FormName').value;

    if (!valid)
        return;

    var model = new FormData();
    model.append("FormName", FormName);

    $.ajax({
        type: "POST",
        url: "/Setting/AuditFilter/CreateSubmit",
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
                CloseModal('1');
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

function DeleteAuditFilter(id) {
    $.ajax({
        type: "Post",
        url: "/Setting/AuditFilter/Delete",
        data: { id: id },
        success: function (data) {
            if (!data.success)
                if (Array.isArray(data.message)) {
                    data.message.forEach(msg => NotyfNews(msg, "error"));
                } else {
                    NotyfNews(data.message, "error");
                }
            else {
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
