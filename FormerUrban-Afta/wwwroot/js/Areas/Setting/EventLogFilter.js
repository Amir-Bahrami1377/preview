$(document).ready(function () {
    var errorElement = document.getElementById("messages");

    if (errorElement) { // بررسی وجود المنت
        var messageJson = errorElement.value;
        var messages = messageJson ? JSON.parse(messageJson) : [];

        if (messages.length > 0) {
            messages.forEach(msg => NotyfNews(msg, "error"));
            document.getElementById("Edit").classList.add("d-none");
        }
        else {
            $(".EventLogFilterReadOnly").prop("disabled", true);
            $(".EventLogFilterReadOnly").prop("readonly", true);

            var cancel = document.getElementById("Cancel");
            if (cancel)
                cancel.classList.add("d-none");

            var submit = document.getElementById("Submit");
            if (submit)
                submit.classList.add("d-none");
        }
    }
});

document.addEventListener("click", function (event) {
    if (event.target.id === "Edit") {
        Edit(event);
    }
    if (event.target.id === "Cancel") {
        Cancel(event);
    }
});

function Edit() {
    document.getElementById("Cancel").classList.remove("d-none");
    document.getElementById("Submit").classList.remove("d-none");
    document.getElementById("Edit").classList.add("d-none");
    $(".EventLogFilterReadOnly").prop("disabled", false);
    $(".EventLogFilterReadOnly").prop("readonly", false);
}

function Cancel() {
    document.getElementById("messages").value = "";
    window.location.href = "/Setting/EventLogFilter";
}