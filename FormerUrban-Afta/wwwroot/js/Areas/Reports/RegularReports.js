$(document).ready(function () {
    ShowDate();
});

document.addEventListener("submit", function (event) {
    if (event.target.id === "RegularReportsForm")
        RegularReportsFormValidatoin(event);
});

function RegularReportsFormValidatoin(event) {
    var valid = true;
    var date = document.getElementById('Date').value;
    if (date != "" && !window.ValidationService.CheckRequired(date, "تاریخ")) valid = false;

    var IP = document.getElementById('Ip').value;
    if (IP != "" && !window.ValidationService.isValidIpRangeOrCidr(IP)) valid = false;

    if (!valid)
        event.preventDefault();
}