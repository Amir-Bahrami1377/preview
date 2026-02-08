$(document).ready(function () {
    var RoleRows = document.querySelectorAll('.RoleRow');
    if (RoleRows.length > 0) {
        SelectUser(RoleRows[0].id);
    }
});

document.addEventListener("click", function (event) {
    let item = event.target.closest(".RoleRow");
    if (item && item.id) {
        SelectUser(item.id);
    }

    if (event.target.id === "SaveRolePermission") {
        SaveRolePermission(event);
    }
});

function SelectUser(id) {
    document.querySelectorAll(".RoleRow").forEach(el => el.classList.remove("selected"));
    let selectedItem = document.getElementById(id);
    if (selectedItem) {
        selectedItem.classList.add("selected");
        GetAllRolePermission(id);
        document.getElementById("SelectedRole").value = id;
    }
}

function GetAllRolePermission(id) {
    $.ajax({
        cache: false,
        type: "POST",
        datatype: "JSON",
        url: '/IdentityUser/RolePermission/GetAllPermission',
        data: { roleName: id },
        success: function (data) {
            document.getElementById("GetAllPermission").innerHTML = data;
        },
        error: function (data) {
            if (data.status === 429)
                window.location.href = "/Error/Error429";
            else
                NotyfNews(data.message, "error");
        }
    });
}

function SaveRolePermission() {
    var permissions = [];
    var roleName = document.getElementById("SelectedRole").value;

    $(".permission-radio:checked").each(function () {
        permissions.push({
            Identity: $(this).data("id"),
            Access: $(this).val() === "true",
            roleName: roleName,
            PermissionName: $(this).data("name"),
        });
    });

    $.ajax({
        url: "/IdentityUser/RolePermission/UpdateUserPermissions",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(permissions),
        success: function (data) {
            if (data.success) {
                //NotyfNews("", "success");
                location.reload();
            }
            else
                NotyfNews("", "error");
        },
        error: function (data) {
            if (data.status === 429)
                window.location.href = "/Error/Error429";
            else
                NotyfNews(data.message, "error");
        }
    });
}

document.addEventListener("keyup", function (event) {
    if (event.target.id === "RoleSearch") {
        SearchRoleSearch(event);
    }

    if (event.target.id === "TypePermissionSearch") {
        SearchTypePermission(event);
    }
});
function SearchTypePermission(event) {
    var value = event.target.value.toLowerCase();
    var rows = document.querySelectorAll("#RolePermissionTable tbody tr");
    rows.forEach(function (row) {
        var text = row.textContent || row.innerText;
        if (text && text.toLowerCase().indexOf(value) > -1) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}
function SearchRoleSearch(event) {
    var value = event.target.value.toLowerCase();
    var rows = document.querySelectorAll("#RoleTable tbody tr");
    rows.forEach(function (row) {
        var text = row.textContent || row.innerText;
        if (text && text.toLowerCase().indexOf(value) > -1) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}