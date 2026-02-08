$(document).ready(function () {
    ShowDate();
});

document.addEventListener("submit", function (event) {
    if (event.target.id === "SmsReportsForm")
        SmsReportsFormValidatoin(event);
});

function SmsReportsFormValidatoin(event) {
    var valid = true;

    var date = document.getElementById('Date').value;
    if (date != "" && !window.ValidationService.CheckRequired(date, "تاریخ")) valid = false;

    var Mobile = document.getElementById('Mobile').value;
    if (Mobile != "" && !window.ValidationService.CheckRequired(Mobile, "شماره موبایل")) valid = false;
    if (Mobile != "" && !ValidationService.IsDigitsOnly(Mobile, "شماره موبایل")) valid = false;
    if (Mobile != "" && !window.ValidationService.CheckMaxNumberLength(Mobile, 12, "طول شماره موبایل")) valid = false;

    if (!valid)
        event.preventDefault();
}