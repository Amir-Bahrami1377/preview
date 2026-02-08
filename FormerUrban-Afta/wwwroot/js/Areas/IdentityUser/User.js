document.addEventListener("click", function (event) {
    if (event.target.id === "CreateUser") {
        ShowModal('/IdentityUser/User/CreateUser', '');
    }
});

document.addEventListener("click", function (event) {
    if (event.target.id === "CreateUserBtnSubmit")
        UserFormValidatoin();

    if (event.target.id === "EditUserBtnSubmit") 
        UserFormValidatoin();

    let target = event.target.closest("#PasswordEye, #PasswordEyeBtn");
    if (target) {
        createpassword("PasswordHash", target);
    }

    let repeatTarget = event.target.closest("#PasswordRepeatEye, #PasswordEyeRepeatBtn");
    if (repeatTarget) {
        createpassword("RepeatPassword", repeatTarget);
    }

    let item = event.target.closest(".UserEditRow");
    if (item && item.id) {
        var userName = document.getElementById(item.id).getAttribute("data-username");
        EditUser(userName);
    }
});

document.addEventListener("input", function (event) {
    switch (event.target.id) {
        case "PasswordHash":
            PasswordStrength("PasswordHash", "password-strength-bar");
            break;
    }
});

function UserFormValidatoin() {
    var valid = true;
    var OldUserName = document.getElementById("oldUserName").value;
    var Id = document.getElementById("Id").value;
    
    var Name = document.getElementById("Name").value;
    if (!window.ValidationService.CheckRequired(Name, "نام")) valid = false;
    if (Name != "" && !window.ValidationService.IsPersianLetters(Name, "نام")) valid = false;
    if (Name != "" && !window.ValidationService.SanitizeAndValidateInput(Name, "نام")) valid = false;
    if (Name != "" && !window.ValidationService.CheckMaxLength(Name, 200, "نام")) valid = false;

    var Family = document.getElementById("Family").value;
    if (!window.ValidationService.CheckRequired(Family, "نام خانوادگی")) valid = false;
    if (Family != "" && !window.ValidationService.IsPersianLetters(Family, "نام خانوادگی")) valid = false;
    if (Family != "" && !window.ValidationService.SanitizeAndValidateInput(Family, "نام خانوادگی")) valid = false;
    if (Family != "" && !window.ValidationService.CheckMaxLength(Family, 200, "نام خانوادگی")) valid = false;

    var UserName = document.getElementById("UserName").value;
    if (!window.ValidationService.CheckRequired(UserName, "نام کاربری")) valid = false;
    if (UserName != "" && !window.ValidationService.SanitizeAndValidateInput(UserName, "نام کاربری")) valid = false;
    if (UserName != "" && !window.ValidationService.CheckMinLength(UserName, 3, "نام کاربری")) valid = false;
    if (UserName != "" && !window.ValidationService.CheckMaxLength(UserName, 200, "نام کاربری")) valid = false;

    var PasswordHash = document.getElementById("PasswordHash").value;
    //if (OldUserName == "" && !window.ValidationService.CheckRequired(PasswordHash, "رمز عبور")) valid = false;
    //if (PasswordHash != "" && !window.ValidationService.SanitizeAndValidateInput(PasswordHash, "رمز عبور")) valid = false;
    //if (PasswordHash != "" && !window.ValidationService.CheckMinLength(PasswordHash, 12, "رمز عبور")) valid = false;
    //if (PasswordHash != "" && !window.ValidationService.CheckMaxLength(PasswordHash, 128, "رمز عبور")) valid = false;
    //if (PasswordHash != "" && !window.ValidationService.isValidpassword(PasswordHash)) valid = false;

    var RepeatPassword = document.getElementById("RepeatPassword").value;
    //if (OldUserName == "" && !window.ValidationService.CheckRequired(RepeatPassword, "تکرار رمز عبور")) valid = false;
    //if (RepeatPassword != "" && !window.ValidationService.SanitizeAndValidateInput(RepeatPassword, "تکرار رمز عبور")) valid = false;
    //if (RepeatPassword != "" && !window.ValidationService.CheckMinLength(RepeatPassword, 12, "تکرار رمز عبور")) valid = false;
    //if (RepeatPassword != "" && !window.ValidationService.CheckMaxLength(RepeatPassword, 128, "تکرار رمز عبور")) valid = false;

    var selectedRoles = document.getElementById("Role").selectedOptions;
    var Role = Array.from(selectedRoles).map(option => option.value);
    if (!Role) {
        NotyfNews("لطفا نقش را وارد کنید!","error");
        valid = false;
    }
    
    var Email = document.getElementById("Email").value;
    if (Email != "" && !ValidationService.IsValidEmail(Email)) valid = false;
    if (Email != "" && !ValidationService.CheckMaxLength(Email, 320, "ایمیل")) valid = false;
    if (Email != "" && !window.ValidationService.SanitizeAndValidateInput(Email, "ایمیل")) valid = false;

    var PhoneNumber = document.getElementById("PhoneNumber").value;
    if (!window.ValidationService.CheckRequired(PhoneNumber, "شماره همراه")) valid = false;
    if (PhoneNumber != "" && !ValidationService.IsValidMobileNumber(PhoneNumber)) valid = false;

    if (PasswordHash !== RepeatPassword) {
        NotyfNews("رمز عبور وارد شده با تکرار رمز عبور مطابقت ندارد!", "error");
        valid = false;
    }

    if (!valid)
        return false;

    var model = new FormData();
    model.append("OldUserName", OldUserName);
    model.append("Id", Id);
    model.append("Name", Name);
    model.append("Family", Family);
    model.append("Email", Email);
    model.append("PhoneNumber", PhoneNumber);
    model.append("UserName", UserName);
    model.append("PasswordHash", PasswordHash);
    model.append("RepeatPassword", RepeatPassword);
    Role.forEach(function (role) {
        model.append("Role", role);
    });
    
    var url = OldUserName ? 'UpdateUserSubmit' : 'CreateUserSubmit';

    $.ajax({
        type: "POST",
        datatype: "JSON",
        url: `/IdentityUser/User/${url}`,
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

function EditUser(userName) {
    var model = new FormData();
    model.append("userName", userName);

    ShowModal('/IdentityUser/User/UpdateUser', model);
}
