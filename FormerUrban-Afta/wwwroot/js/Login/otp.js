
// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "OtpForm")
        OtpFormValidation(event);
});

function OtpFormValidation(e) {
    var valid = true;
    var Code = document.getElementById("Code").value;
    if (!window.ValidationService.CheckRequired(Code, "کد OTP")) valid = false;
    if (Code != "" && !window.ValidationService.CheckLength(Code, 6, "کد OTP")) valid = false;

    var captchaCode = document.getElementById("captchaCode").value;
    if (!window.ValidationService.CheckRequired(captchaCode, "کد کپچا")) valid = false;
    if (captchaCode != "" && !window.ValidationService.CheckLength(captchaCode, 5, "کد کپچا")) valid = false;
    if (captchaCode != "" && !window.ValidationService.SanitizeAndValidateInput(captchaCode, "کد کپچا")) valid = false;

    if (!valid)
        e.preventDefault();
}
// #endregion