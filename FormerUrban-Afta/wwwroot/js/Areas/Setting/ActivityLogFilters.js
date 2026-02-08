document.addEventListener("click", function (event) {
    if (event.target.id === "CreateActivityLogFilters")
        CreateActivityLogFilters("");

    if (event.target.id === "CreateActivityLogFiltersSubmit")
        ActivityLogFiltersFormValidatoin("");

    if (event.target.id === "EditActivityLogFiltersSubmit")
        ActivityLogFiltersFormValidatoin("");


    let item = event.target.closest(".ActivityLogFiltersEditRow");
    if (item && item.id) {
        var identity = document.getElementById(item.id).getAttribute("data-identity");
        CreateActivityLogFilters(identity);
    }

    let item2 = event.target.closest(".ActivityLogFiltersDeleteRow");
    if (item2 && item2.id) {
        var identity = document.getElementById(item2.id).getAttribute("data-identity");
        DeleteActivityLogFilters(identity);
    }
});

function CreateActivityLogFilters(Id) {

    var id = Id ?? "";

    var model = new FormData();
    model.append("identity", id);

    var url = Id > 0 ? "Edit" : "Create";

    ShowModal(`/Setting/ActivityLogFilters/${url}`, model);
}

function ActivityLogFiltersFormValidatoin() {
    var valid = true;

    var Identity = document.getElementById('Identity').value;

    var FormName = document.getElementById('FormName').value;
    var TableName = $("#FormName option:selected").text();
    var GetStatus = $("#GetStatus:checked").val() == undefined ? false : true;
    var AddStatus = $("#AddStatus:checked").val() == undefined ? false : true;
    var UpdateStatus = $("#UpdateStatus:checked").val() == undefined ? false : true;
    var DeleteStatus = $("#DeleteStatus:checked").val() == undefined ? false : true;

    if (!valid)
        return;

    var model = new FormData();
    model.append("Identity", Identity);
    model.append("FormName", FormName);
    model.append("TableName", TableName);
    model.append("GetStatus", GetStatus);
    model.append("AddStatus", AddStatus);
    model.append("UpdateStatus", UpdateStatus);
    model.append("DeleteStatus", DeleteStatus);

    var url = "CreateSubmit";
    if (Identity > 0)
        url = "EditSubmit";

    $.ajax({
        type: "POST",
        url: `/Setting/ActivityLogFilters/${url}`,
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

function DeleteActivityLogFilters(id) {
    $.ajax({
        type: "Post",
        url: `/Setting/ActivityLogFilters/Delete`,
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