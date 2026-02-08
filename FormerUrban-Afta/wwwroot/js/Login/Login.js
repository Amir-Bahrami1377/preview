$(document).ready(function () {
    var message = $('#Message').val();
    if (message) {

        if (message == "-") {
            NotyfNews("", "success");
            return;
        }

        if (message.indexOf(" / ") === -1)
            NotyfNews(message, "error");
        else {
            var messages = message.split(" / ");
            messages.forEach(function (msg) {
                NotyfNews(msg, "error");
            });
        }
    }
});

document.addEventListener("click", function (event) {

    if (event.target.id === "RefreshCapcha")
        refreshCaptcha();

    let Password = event.target.closest("#PasswordEye, #PasswordEyeBtn");
    if (Password) {
        createpassword("Password", Password);
    }
});

function refreshCaptcha() {
    var captchaImage = document.getElementById("captchaImage");
    captchaImage.src = '/Login/GenerateCaptcha?' + new Date().getTime();
}

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "LoginForm")
        LoginFormValidation(event);

    if (event.target.id === "ForgetPasswordForm")
        ForgetPasswordFormValidation(event);

    if (event.target.id === "ChangePasswordForm")
        ChangePasswordFormValidation(event);
});

function LoginFormValidation(e) {

    var valid = true;
    var userName = document.getElementById("UserName").value;
    var password = document.getElementById("Password").value;

    if (!window.ValidationService.CheckRequired(userName, "نام کاربری")) valid = false;
    if (userName != "" && !window.ValidationService.SanitizeAndValidateInput(userName, "نام کاربری")) valid = false;
    if (userName != "" && !window.ValidationService.CheckMinLength(userName, 3, "نام کاربری")) valid = false;
    if (userName != "" && !window.ValidationService.CheckMaxLength(userName, 200, "نام کاربری")) valid = false;

    if (!window.ValidationService.CheckRequired(password, "رمز عبور")) valid = false;
    if (password != "" && !window.ValidationService.SanitizeAndValidateInput(password, "رمز عبور")) valid = false;
    if (password != "" && !window.ValidationService.CheckMinLength(password, 12, "رمز عبور")) valid = false;
    if (password != "" && !window.ValidationService.CheckMaxLength(password, 128, "رمز عبور")) valid = false;

    if (!valid)
        e.preventDefault();
}

function ForgetPasswordFormValidation(e) {
    var valid = true;

    var mobile = document.getElementById("mobile").value;
    if (!window.ValidationService.CheckRequired(mobile, "شماره همراه")) valid = false;
    if (mobile != "" && !ValidationService.IsValidMobileNumber(mobile)) valid = false;

    var CaptchaCode = document.getElementById("CaptchaCode").value;
    if (!window.ValidationService.CheckRequired(CaptchaCode, "کد کپچا")) valid = false;
    if (CaptchaCode != "" && !window.ValidationService.CheckLength(CaptchaCode, 5, "کد کپچا")) valid = false;
    if (CaptchaCode != "" && !window.ValidationService.SanitizeAndValidateInput(CaptchaCode, "کد کپچا")) valid = false;

    if (!valid)
        e.preventDefault();
}

function ChangePasswordFormValidation(e) {
    var valid = true;

    var NewPassword = document.getElementById("NewPassword").value;
    if (!window.ValidationService.CheckRequired(NewPassword, "رمز عبور جدید")) valid = false;
    if (NewPassword != "" && !window.ValidationService.SanitizeAndValidateInput(NewPassword, "رمز عبور جدید")) valid = false;
    if (NewPassword != "" && !window.ValidationService.CheckMinLength(NewPassword, 12, "رمز عبور جدید")) valid = false;
    if (NewPassword != "" && !window.ValidationService.CheckMaxLength(NewPassword, 128, "رمز عبور جدید")) valid = false;
    if (NewPassword != "" && !window.ValidationService.isValidpassword(NewPassword)) valid = false;

    var RepeatPassword = document.getElementById("RepeatPassword").value;
    if (!window.ValidationService.CheckRequired(RepeatPassword, "تکرار رمز عبور جدید")) valid = false;
    if (RepeatPassword != "" && !window.ValidationService.SanitizeAndValidateInput(RepeatPassword, "تکرار رمز عبور جدید")) valid = false;
    if (RepeatPassword != "" && !window.ValidationService.CheckMinLength(RepeatPassword, 12, "تکرار رمز عبور جدید")) valid = false;
    if (RepeatPassword != "" && !window.ValidationService.CheckMaxLength(RepeatPassword, 128, "تکرار رمز عبور جدید")) valid = false;

    if (NewPassword !== RepeatPassword) {
        NotyfNews("رمز عبور جدید وارد شده با تکرار رمز عبور مطابقت ندارد!!", "error");
        valid = false;
    }

    if (!valid)
        e.preventDefault();
}

// #endregion