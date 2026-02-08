$(function () {
    var dataTable = $('.DataTable');
    if (dataTable.length > 0)
        $('.DataTable').DataTable({
            order : [],
        });

    var dataTable = $('.AmardDataTable');
    if (dataTable.length > 0)
        $('.AmardDataTable').DataTable({
            order: [],
        });

    var message = $('#message').val();
    if (message)
        NotyfNews(message, "success");

    var ErrorMessage = $('#ErrorMessage').val();
    if (ErrorMessage)
        NotyfNews(ErrorMessage, "error");

    var SuccessMessage = $('#SuccessMessage').val();
    if (SuccessMessage)
        NotyfNews(SuccessMessage, "success");

    let validateMessage = $('#ValidateMessage').val();
    if (validateMessage) {
        let validateSplit = validateMessage.split("/");
        validateSplit.forEach(msg => {
            if (msg.trim()) {
                NotyfNews(msg.trim(), "error");
            }
        });
    }
 
    //ShowDate();
});

// #region Notyf
function NotyfNews(message, type) {
    var bg = "#198754";
    var text = "";
    switch (type) {
        case "success":
            bg = "#198754";
            text = "عملیات با موفقیت انجام شد";
            break;
        case "error":
            bg = "#f44336";
            text = "عملیات با خطا مواجه شده است. لطفا با پشتیبانی تماس بگیرید!!";
            break;

        case "warning":
            bg = "#ddc900";
            text = "لطفا با پشتیبانی تماس بگیرید!!";
            break;
    }

    if (message)
        text = message;

    var notyf = new Notyf({
        duration: 0,
        position: { x: 'left', y: 'top' },
        dismissible: true,
        types: [
            {
                type: 'success',
                background: bg,
                dismissible: true,
                icon: false
            }
        ]
    });

    const notification = notyf.success(text);
    setTimeout(() => {
        const elements = document.querySelectorAll('.notyf__toast');
        elements.forEach((element, index) => {
            let hideTimeout;
            element.style.marginTop = `${10 + (index * 100)}px`;
            element.addEventListener('mouseover', () => {
                clearTimeout(hideTimeout);
            });

            element.addEventListener('mouseout', () => {
                hideTimeout = setTimeout(() => {
                    element.remove();
                }, 3000);
            });

            hideTimeout = setTimeout(() => {
                element.remove();
            }, 3000);
        });
    }, 100);
}

// #endregion

//#region Password


let createpassword = (type, ele) => {
    let input = document.getElementById(type);
    if (input) {
        input.type = input.type === "password" ? "text" : "password";
    }

    let icon = ele.tagName === "I" ? ele : ele.querySelector("i");
    if (icon) {
        let iconClasses = icon.classList;
        if (iconClasses.contains("ri-eye-line")) {
            iconClasses.remove("ri-eye-line");
            iconClasses.add("ri-eye-off-line");
        } else {
            iconClasses.add("ri-eye-line");
            iconClasses.remove("ri-eye-off-line");
        }
    }
};

function PasswordStrength(InputId, ProgressId) {
    const passwordInput = document.getElementById(InputId);
    const password = passwordInput.value.trim();
    const progressBar = document.getElementById(ProgressId);

    if (!progressBar) return;

    if (password === "") {
        progressBar.className = 'progress-bar';
        progressBar.style.width = "0%";
        progressBar.innerHTML = "";
        return;
    }

    const result = zxcvbn(password);
    const strength = result.score;
    const strengthColors = ['bg-danger', 'bg-warning', 'bg-primary', 'bg-info', 'bg-success'];
    const messages = ['خیلی ضعیف', 'ضعیف', 'متوسط', 'خوب', 'عالی'];

    progressBar.className = 'progress-bar';
    progressBar.style.width = `${(strength + 1) * 20}%`;
    progressBar.classList.add(strengthColors[strength]);
    progressBar.innerHTML = messages[strength];
}

function ChangePassword() {
    ShowModal('/IdentityUser/User/ChangePassword', '')
}

document.addEventListener("click", function (event) {
    if (event.target.id === "ChangePassword")
        ChangePassword();

    if (event.target.id === "ChangePasswordSubmitBtn")
        ChangePasswordSubmit();

    let targetOld = event.target.closest("#OldPasswordRepeatEye, #OldPasswordEyeBtn");
    if (targetOld) {
        createpassword("OldPassword", targetOld);
    }

    let targetNew = event.target.closest("#NewPasswordEye, #NewPasswordEyeBtn");
    if (targetNew) {
        createpassword("NewPassword", targetNew);
    }

    let targetRepeat = event.target.closest("#RepeatPasswordRepeatEye, #RepeatPasswordEyeBtn");
    if (targetRepeat) {
        createpassword("RepeatPassword", targetRepeat);
    }
});

document.addEventListener("input", function (event) {
    switch (event.target.id) {
        case "NewPassword":
            PasswordStrength("NewPassword", "new-password-strength-bar");
            break;
    }
});

document.addEventListener("blur", function (event) {
    switch (event.target.id) {
        case "NewPassword":
            PasswordStrength("NewPassword", "new-password-strength-bar");
            break;
    }
}, true);

function ChangePasswordSubmit() {
    var valid = true;

    var OldPassword = document.getElementById("OldPassword").value;
    if (!window.ValidationService.CheckRequired(OldPassword, "رمز عبور قدیم")) valid = false;
    if (OldPassword != "" && !window.ValidationService.SanitizeAndValidateInput(OldPassword, "رمز عبور قدیم")) valid = false;
    if (OldPassword != "" && !window.ValidationService.CheckMinLength(OldPassword, 12, "رمز عبور قدیم")) valid = false;
    if (OldPassword != "" && !window.ValidationService.CheckMaxLength(OldPassword, 128, "رمز عبور قدیم")) valid = false;
    if (OldPassword != "" && !window.ValidationService.isValidpassword(OldPassword)) valid = false;

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
        return false;

    var model = new FormData();
    model.append("OldPassword", OldPassword);
    model.append("NewPassword", NewPassword);
    model.append("RepeatPassword", RepeatPassword);

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: '/IdentityUser/User/ChangePasswordSubmit',
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

// #endregion

//#region Modal

function ShowModal(url, model) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            type: "POST",
            data: model,
            url: url,
            contentType: false,
            processData: false,
            success: function (data) {
                var modal = new bootstrap.Modal($('#Modal'));
                $('#ModalContent').empty();
                $('#ModalContent').append(data);
                modal.show();
                
                var data = $(".AmardDate");
                if (data.length > 0)
                    ShowDate();

                var dataTime = $(".AmardDateTime");
                if (dataTime.length > 0)
                    ShowTimeDate();

                var role = $('#Role');
                if (role.length > 0)
                    role.select2();

                resolve(true);  // عملیات موفقیت‌آمیز
            },
            error: function (err) {
                if (err.status === 401)
                    window.location.href = "/Login/Logout"; // مسیر لاگین خودت
                else if (err.status === 429)
                    window.location.href = "/Error/Error429"; // مسیر لاگین خودت
                else
                    NotyfNews(err.message, "error");

            }
        });
    });
}
function CloseModal(notyf) {
    var modal = bootstrap.Modal.getInstance($('#Modal'));
    if (modal)
        modal.hide();

    //if (notyf == '1')
    //    NotyfNews("", "success");
}

$('#Modal').on('hidden.bs.modal', function () {
    var DatePeakers = $('.AmardDate');
    if(DatePeakers.length > 0)
        $('.AmardDate').pDatepicker('hide');
});

// #endregion

//#region back to top

const scrollToTop = document.querySelector(".scrollToTop");
const $rootElement = document.documentElement;
const $body = document.body;
window.onscroll = () => {
    const scrollTop = window.scrollY || window.pageYOffset;
    const clientHt = $rootElement.scrollHeight - $rootElement.clientHeight;
    if (window.scrollY > 100) {
        scrollToTop.style.display = "flex";
    } else {
        scrollToTop.style.display = "none";
    }
};
scrollToTop.onclick = () => {
    window.scrollTo(0, 0);
};

// #endregion

$.ajaxSetup({
    beforeSend: function (xhr, settings) {
        if (settings.type == 'POST' || settings.type == 'PUT' || settings.type == 'DELETE') {
            xhr.setRequestHeader('X-CSRF-TOKEN', $('meta[name=csrf-token]').attr('content'));
        }
    },
    error: function (data) {
        if (data.status === 400)
            window.location.href = "/Error/Error400";
        if (data.status === 401)
            window.location.href = "/Error/Error401";
        if (data.status === 403)
            window.location.href = "/Error/Error403";
        if (data.status === 404)
            window.location.href = "/Error/Error404";
        if (data.status === 429)
            window.location.href = "/Error/Error429";
        if (data.status === 500)
            window.location.href = "/Error/Error500";
        else
            NotyfNews(data.message || "خطایی رخ داده است.", "error");
    }
});
