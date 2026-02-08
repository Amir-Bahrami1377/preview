$(document).ready(function () {
    var userRows = document.querySelectorAll('.UserRow');
    if (userRows.length > 0) {
        SelectUser(userRows[0].id);
    }
});

document.addEventListener("click", function (event) {
    let item = event.target.closest(".UserRow");
    if (item && item.id) {
        SelectUser(item.id);
    }
    if (event.target.id === "SaveUserPermission") {
        SaveUserPermission(event);
    }
});
function SelectUser(id) {
    document.querySelectorAll(".UserRow").forEach(el => el.classList.remove("selected"));
    let selectedItem = document.getElementById(id);
    if (selectedItem) {
        selectedItem.classList.add("selected");
        GetAllUserPermission(id);
        document.getElementById("SelectedUserName").value = id;
    }
}
function GetAllUserPermission(id) {
    $.ajax({
        cache: false,
        type: "POST",
        datatype: "JSON",
        url: '/IdentityUser/UserPermission/GetAllPermission',
        data: { userName: id },
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


document.addEventListener("keyup", function (event) {
    if (event.target.id === "UserNameSearch") {
        SearchUserName(event);
    }

    if (event.target.id === "TypePermissionSearch") {
        SearchTypePermission(event);
    }
});
function SearchTypePermission(event) {
    var value = event.target.value.toLowerCase();
    var rows = document.querySelectorAll("#UserPermissionTable tbody tr");
    rows.forEach(function (row) {
        var text = row.textContent || row.innerText;
        if (text && text.toLowerCase().indexOf(value) > -1) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}
function SearchUserName(event) {
    var value = event.target.value.toLowerCase();
    var rows = document.querySelectorAll("#UserTable tbody tr");
    rows.forEach(function (row) {
        var text = row.textContent || row.innerText;
        if (text && text.toLowerCase().indexOf(value) > -1) {
            row.style.display = "";
        } else {
            row.style.display = "none";
        }
    });
}

function SaveUserPermission() {
    var permissions = [];
    var userName = document.getElementById("SelectedUserName").value;

    $(".permission-radio:checked").each(function () {
        permissions.push({
            Identity: $(this).data("id"),
            Access: $(this).val() === "true",
            UserName: userName,
            Permission_Name : $(this).data("name"),
        });
    });

    $.ajax({
        url: "/IdentityUser/UserPermission/UpdateUserPermissions",
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