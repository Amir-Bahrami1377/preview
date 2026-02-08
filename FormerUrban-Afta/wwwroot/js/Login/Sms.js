// اجرا کردن کد هنگام لود صفحه
document.addEventListener("DOMContentLoaded", function () {
    GetSmsCode();
});

function GetSmsCode(expireDate) {
    const resendBtn = document.getElementById("resendBtn"); // دکمه ارسال مجدد
    const countdownElement = document.getElementById("countdown"); // المنت ثانیه شمار
    const timerText = document.getElementById("timerText"); // المنت پدر ثانیه شمار


    const smsExpireTimeString = expireDate ?? document.getElementById("ExpireTime").value; // گرفتن زمان انقضا از سرور (فرمت رشته‌ای)
    const expireTime = convertToDate(smsExpireTimeString); // تبدیل رشته تاریخ به یک شیء Date در جاوااسکریپت
    let countdown = getCountdown(expireTime) + 1;// محاسبه زمان باقی‌مانده
    startCountdown(countdown, resendBtn, countdownElement, timerText);    // شروع تایمر
}

// تبدیل تاریخ رشته‌ای به یک شیء Date
function convertToDate(smsExpireTimeString) {
    const [datePart, timePart] = smsExpireTimeString.split(" ");
    const [day, month, year] = datePart.split("/").map(Number);
    const [hour, minute, second] = timePart.split(":").map(Number);

    return new Date(year, month - 1, day, hour, minute, second); // month در جاوااسکریپت از صفر شروع می‌شود
}

// محاسبه زمان باقی‌مانده به ثانیه
function getCountdown(expireTime) {
    var cut = Math.floor((expireTime - new Date()) / 1000); // محاسبه زمان باقی‌مانده به ثانیه
    return cut;
}

// بروزرسانی UI با زمان باقی‌مانده
function updateUI(countdown, countdownElement, timerText) {
    countdownElement.textContent = countdown;
    timerText.innerHTML = "زمان انقضای کد (ثانیه) : ";
    document.getElementById("countdown").innerHTML = countdown;
}

// شروع تایمر
function startCountdown(duration, resendBtn, countdownElement, timerText) {
    let countdown = duration;
    resendBtn.disabled = true;
    //updateUI(countdown, countdownElement, timerText);  // بروزرسانی UI با زمان باقی‌مانده

    let timer = setInterval(() => {
        countdown--;
        if (countdown > 0)
            updateUI(countdown, countdownElement, timerText); // بروزرسانی UI با هر ثانیه
        else {
            clearInterval(timer);
            resendBtn.disabled = false;
            timerText.innerText = "می‌توانید دوباره کد را دریافت کنید.";
            document.getElementById("countdown").innerHTML = "";
        }
    }, 1000);
}

// ارسال درخواست برای ارسال مجدد کد
document.addEventListener("click", function (event) {
    if (event.target.id === "resendBtn") {
        resendSmsCode(event);
    }
});
function resendSmsCode() {
    $("#resendBtn").prop("disabled", true);
    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    var userName = $("#UserName").val()

    $.ajax({
        url: '/Login/ResendSmsCode',
        method: 'POST',
        data: { userName: userName },
        success: function (data) {
            if (data.success) {
                $("#ExpireTime").val(data.expireTime)
                GetSmsCode(data.expireTime);
            }
            else
                NotyfNews(data.error, "error");
        },
        error: function (xhr, status, error) {
            alert("خطا در ارسال مجدد کد");
            console.error("جزئیات خطا:", status, error);
            $("#resendBtn").prop("disabled", false);
        }
    });
}

// #region Validation

document.addEventListener("submit", function (event) {
    if (event.target.id === "SmsForm")
        SmsFormValidation(event);
});

function SmsFormValidation(e) {
    var valid = true;

    var sms = document.getElementById("sms").value;
    if (!window.ValidationService.CheckRequired(sms, "کد پیامک شده")) valid = false;
    if (sms != "" && !window.ValidationService.CheckLength(sms, 6, "کد پیامک شده")) valid = false;

    var captchaCode = document.getElementById("captchaCode").value;
    if (!window.ValidationService.CheckRequired(captchaCode, "کد کپچا")) valid = false;
    if (captchaCode != "" && !window.ValidationService.CheckLength(captchaCode, 5, "کد کپچا")) valid = false;
    if (captchaCode != "" && !window.ValidationService.SanitizeAndValidateInput(captchaCode, "کد کپچا")) valid = false;

    if (!valid) {
        const eDate = document.getElementById("ExpireTime").value;
        GetSmsCode(eDate)
        e.preventDefault()
    }
}
// #endregion