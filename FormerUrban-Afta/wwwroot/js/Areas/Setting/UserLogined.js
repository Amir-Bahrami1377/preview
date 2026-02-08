$(document).ready(function () {
    ShowDate();
});

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "UserLoginedForm")
        UserLoginedFormValidatoin(event);
});

function UserLoginedFormValidatoin(e) {
    var valid = true;

    var FromDateTime = document.getElementById("FromDateTime").value;
    if (FromDateTime != "" && !window.ValidationService.IsValidPersianDate(FromDateTime, "تاریخ شروع")) valid = false;

    var ToDateTime = document.getElementById("ToDateTime").value;
    if (ToDateTime != "" && !window.ValidationService.IsValidPersianDate(ToDateTime, "تاریخ پایان")) valid = false;

    var ArrivalDate = document.getElementById("ArrivalDate").value;
    if (ArrivalDate != "" && !window.ValidationService.IsValidPersianDate(ArrivalDate, "تاریخ ورود")) valid = false;

    var DepartureDate = document.getElementById("DepartureDate").value;
    if (DepartureDate != "" && !window.ValidationService.IsValidPersianDate(DepartureDate, "تاریخ خروج")) valid = false;

    var userName = document.getElementById("UserName").value;
    if (userName != "" && !window.ValidationService.SanitizeAndValidateInput(userName, "نام کاربری")) valid = false;
    if (userName != "" && !window.ValidationService.CheckMinLength(userName, 3, "نام کاربری")) valid = false;
    if (userName != "" && !window.ValidationService.CheckMaxLength(userName, 200, "نام کاربری")) valid = false;
    
    var Ip = document.getElementById("Ip").value;
    if (Ip != "" && !window.ValidationService.isValidIp(Ip)) valid = false;

    if (!valid)
        e.preventDefault();
}

// #endregion