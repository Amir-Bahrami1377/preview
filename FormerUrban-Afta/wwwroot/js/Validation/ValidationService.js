const ValidationService = {

    CheckRequired: function (value, fieldName) {
        if (!value || value.trim() === "" || value == "0") {
            ValidationService.ShowError(ValidationMessage.Required(fieldName));
            return false;
        }
        return true;
    },

    IsDigitsOnly: function (value, fieldName) {
        let digiValid = /^\d+$/.test(value);
        if (digiValid)
            return true;

        ValidationService.ShowError(ValidationMessage.OnlyDigits(fieldName));
        return false;
    },

    IsValidCodeNosazi: function (value) {
        let cdValid = /^\d+(-\d+){6}$/.test(value);
        if (cdValid)
            return true;

        ValidationService.ShowError(ValidationMessage.InvalidCodeNosazi);
        return false;
    },

    CheckLength: function (value, Length, fieldName) {
        if (value.length != Length) {
            ValidationService.ShowError(ValidationMessage.Length(fieldName, Length))
            return false;
        }
        return true;
    },

    CheckMinLength: function (value, minLength, fieldName) {
        if (value.length < minLength) {
            ValidationService.ShowError(ValidationMessage.MinLength(fieldName, minLength))
            return false;
        }
        return true;
    },

    CheckMaxLength: function (value, maxLength, fieldName) {
        if (value.length > maxLength) {
            ValidationService.ShowError(ValidationMessage.MaxLength(fieldName, maxLength))
            return false;
        }
        return true;
    },

    CheckMaxNumberLength: function (value, maxLength, fieldName) {
        if (value.length > maxLength) {
            ValidationService.ShowError(ValidationMessage.MaxNumberLength(fieldName, maxLength))
            return false;
        }
        return true;
    },

    Equal: function (value, Equal, fieldName) {
        if (value != Equal) {
            ValidationService.ShowError(ValidationMessage.Equal(fieldName, Equal))
            return false;
        }
        return true;
    },

    NotEqual: function (value, Equal, fieldName) {
        if (value == Equal) {
            ValidationService.ShowError(ValidationMessage.NotEqual(fieldName, Equal))
            return false;
        }
        return true;
    },

    MoreThan: function (value, max, fieldName) {
        if (value < max) {
            ValidationService.ShowError(ValidationMessage.MoreThan(max, fieldName))
            return false;
        }
        return true;
    },

    MoreThan2: function (value, max, fieldName) {
        if (value <= max) {
            ValidationService.ShowError(ValidationMessage.MoreThan(max, fieldName))
            return false;
        }
        return true;
    },

    IsPersianLetters: function (input, fieldName) {
        const regex = /^[\u0600-\u06FF\s]*$/;
        if (!regex.test(input)) {
            ValidationService.ShowError(ValidationMessage.IsPersianLetters(fieldName))
            return false;
        }
        return true;
    },

    isAlphanumeric: function (input, fieldName) {
        const regex = /^[a-zA-Z0-9\u0600-\u06FF ,./\-]*$/;
        if (!regex.test(input)) {
            ValidationService.ShowError(ValidationMessage.isAlphanumeric(fieldName))
            return false;
        }
        return true;
    },

    SanitizeAndValidateInput: function (input, fieldName) {
        if (!input || input.trim() === '') return true;

        const original = input.trim();
        const dangerousChars = ["<", ">", "\"", "'", ";", "%", "(", ")", "+", "=", "\\", "--", "`", "/"];

        // حذف کاراکترهای خطرناک
        dangerousChars.forEach(ch => {
            const escaped = ch.replace(/[.*+?^${}()|[\]\\]/g, '\\$&'); // escape برای regex
            input = input.replace(new RegExp(escaped, 'g'), '');
        });

        // حذف فاصله‌های تکراری و کاراکترهای کنترل
        input = input.replace(/\s{2,}/g, ' ').trim();
        input = input.replace(/[\x00-\x1F\x7F]/g, '');

        const scriptOrHtmlPattern = /<script[\s\S]*?>|<\/script>|<.*?>/i;
        const urlPattern = /(http|https|ftp|www)\S*/i;
        const sqlPattern = /\b(SELECT|INSERT|DELETE|UPDATE|DROP|ALTER|CREATE|EXEC|UNION|CAST|DECLARE|TRUNCATE|MERGE|GRANT|REVOKE|COMMIT|ROLLBACK|SAVEPOINT|SHUTDOWN)\b/i;
        const specialCharsPattern = /(--|\bOR\b|\bAND\b|;|'|\(|\))/i;

        if (
            scriptOrHtmlPattern.test(input) ||
            urlPattern.test(input) ||
            sqlPattern.test(input) ||
            specialCharsPattern.test(input)
        ) {
            window.ValidationService.ShowError(ValidationMessage.SanitizeAndValidateInput(fieldName));
            return false;
        }

        if (input === original)
            return true;

        window.ValidationService.ShowError(ValidationMessage.SanitizeAndValidateInput(fieldName));
        return false;
    },

    Between: function (input, one, two, fieldName) {
        if (input < one || input > two) {
            ValidationService.ShowError(ValidationMessage.Between(fieldName, one, two));
            return false;
        }
        return true;
    },

    IsValidTel: function (tel) {
        const pattern = /^(0[1-9]{2,3})?\d{8}$/;
        if (!pattern.test(tel)) {
            ValidationService.ShowError(ValidationMessage.IsValidTel())
            return false;
        }
        return true;
    },

    IsValidPostalCode: function (postalCode) {
        var regex = /^\d{10}$/;
        if (!regex.test(postalCode)) {
            ValidationService.ShowError(ValidationMessage.IsValidPostalCode());
            return false;
        }
        return true;
    },

    AmountIsValidFormat: function (amount, fieldName) {
        var formatted = amount.toString();
        var regex = /^\d+(\/\d{1,2})?$/;
        //var formatted = parseFloat(amount).toFixed(2);
        //var regex = /^\d+(\.\d{1,2})?$/;
        if (!regex.test(formatted)) {
            ValidationService.ShowError(ValidationMessage.ValidAmountFormat(fieldName));
            return false;
        }
        return true;
    },

    IsValidUtmX: function (x) {
        if (x < 200000 || x > 900000) {
            ValidationService.ShowError(ValidationMessage.IsValidUtmX());
            return false;
        }
        return true;
    },

    IsValidUtmY: function (y) {
        if (y < 3300000 || y > 4200000) {
            ValidationService.ShowError(ValidationMessage.IsValidUtmY());
            return false;
        }
        return true;
    },

    IsValidNationalCode: function (nationalCode) {

        if (!/^\d{10}$/.test(nationalCode)) {
            ValidationService.ShowError(ValidationMessage.IsValidNationalCodeLength());
            return false;
        }

        if (/^(\d)\1{9}$/.test(nationalCode)) {
            ValidationService.ShowError(ValidationMessage.IsValidNationalCodeRepet());
            return false;
        }

        var check = parseInt(nationalCode[9]);
        var sum = 0;
        for (var i = 0; i < 9; i++) {
            sum += parseInt(nationalCode[i]) * (10 - i);
        }
        var remainder = sum % 11;

        if (!((remainder < 2 && check === remainder) || (remainder >= 2 && check === 11 - remainder))) {
            ValidationService.ShowError(ValidationMessage.IsValidNationalCode());
            return false;
        }

        return true;
    },

    IsValidMobileNumber: function (mobile) {
        const regex = /^09\d{9}$/;
        if (!regex.test(mobile)) {
            ValidationService.ShowError(ValidationMessage.IsValidMobileNumber());
            return false;
        }
        return true;
    },

    IsValidEmail: function (email) {
        const regex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!regex.test(email)) {
            ValidationService.ShowError(ValidationMessage.IsValidEmail());
            return false;
        }
        return true;
    },

    IsValidPersianDate: function (date, fieldName) {

        if (date.length !== 10) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDate(fieldName));
            return false;
        }

        date = ValidationService.ConvertPersianDigitsToEnglish(date);
        const parts = date.split('/');

        if (parts.length !== 3) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDate(fieldName));
            return false;
        }

        const year = parseInt(parts[0], 10);
        const month = parseInt(parts[1], 10);
        const day = parseInt(parts[2], 10);

        if (isNaN(year)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateYearEmpty(fieldName));
            return false;
        }

        if (isNaN(month)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateMonthEmpty(fieldName));
            return false;
        }

        if (isNaN(day)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateDayEmpty(fieldName));
            return false;
        }

        if (year < 1300 || year > 1499) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateYear(fieldName));
            return false;
        }

        if (month < 1 || month > 12) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateMonth(fieldName));
            return false;
        }

        if (day < 1) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateDay(fieldName));
            return false;
        }

        const daysInMonth = [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];
        let maxDay = daysInMonth[month - 1];

        // بررسی سال کبیسه برای اسفند
        if (month === 12 && ValidationService.IsPersianLeapYear(year)) {
            maxDay = 30;
        }

        if (day > maxDay) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateDay2(fieldName));
            return false;
        }

        return true;
    },

    IsValidBirthCertificateNumber: function (number) {
        const regex = /^\d{1,10}$/;
        if (!regex.test(number)) {
            ValidationService.ShowError(ValidationMessage.IsValidBirthCertificateNumber());
            return false;
        }
        return true;
    },

    IsValidLegalNationalId: function (nationalId/*, options = { strictCheckDigit: false }*/) {
        // Check length
        if (!/^\d{11}$/.test(nationalId)) {
            ValidationService.ShowError(ValidationMessage.IsValidLegalNationalIdLength());
            return false;
        }

        // Check repeated digits
        if (/^(\d)\1{10}$/.test(nationalId)) {
            ValidationService.ShowError(ValidationMessage.IsValidLegalNationalId());
            return false;
        }
        return true;
    },

    isValidIpRangeOrCidr(input) {
        const ipPattern = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;

        function isValidIp(ip) {
            const match = ip.match(ipPattern);
            if (!match) return false;

            return match.slice(1).every(part => {
                const num = parseInt(part, 10);
                return num >= 0 && num <= 255;
            });
        }

        if (input.includes("/")) {
            const parts = input.split('/');
            if (parts.length !== 2) {
                ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
                return false;
            }

            const [ip, cidrStr] = parts;

            if (!isValidIp(ip)) {
                ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
                return false;
            }

            if (!/^\d{1,3}$/.test(cidrStr)) {
                ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
                return false;
            }

            const cidr = parseInt(cidrStr, 10);
            if (cidr < 0 || cidr > 256) {
                ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
                return false;
            }

            return true;
        }

        if (input.includes("-")) {
            const parts = input.split('-');
            if (parts.length !== 2) {
                ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
                return false;
            }

            if (!isValidIp(parts[0]) || !isValidIp(parts[1])) {
                ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
                return false;
            }

            return true;
        }

        if (!isValidIp(input)) {
            ValidationService.ShowError(ValidationMessage.isValidIpRangeOrCidr());
            return false;
        }

        return true;
    },

    isValidPersianDateTime(input, filedName) {
        if (!input || input.trim() === "") return true;

        input = ValidationService.ConvertPersianDigitsToEnglish(input.trim());
        const parts = input.split(' ');

        if (parts.length !== 2) return false;

        let datePart = null;
        let timePart = null;

        // تشخیص اینکه کدام بخش تاریخ است و کدام بخش زمان
        if (parts[0].includes("/") && parts[1].includes(":")) {
            datePart = parts[0];
            timePart = parts[1];
        } else if (parts[1].includes("/") && parts[0].includes(":")) {
            datePart = parts[1];
            timePart = parts[0];
        } else {
            ValidationService.ShowError(ValidationMessage.isValidPersianDateTime(filedName));
            return false;
        }

        // بررسی تاریخ
        const dateParts = datePart.split('/');
        if (dateParts.length !== 3) return false;

        const year = parseInt(dateParts[0], 10);
        const month = parseInt(dateParts[1], 10);
        const day = parseInt(dateParts[2], 10);

        if (isNaN(year)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateYearEmpty(filedName));
            return false;
        }

        if (isNaN(month)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateMonthEmpty(filedName));
            return false;
        }

        if (isNaN(day)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateDayEmpty(filedName));
            return false;
        }

        if (year < 1300 || year > 1499) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateYear(filedName));
            return false;
        }

        if (month < 1 || month > 12) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateMonth(filedName));
            return false;
        }

        if (day < 1) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateDay(fieldName));
            return false;
        }

        const daysInMonth = [31, 31, 31, 31, 31, 31, 30, 30, 30, 30, 30, 29];
        let maxDay = daysInMonth[month - 1];

        // بررسی سال کبیسه برای اسفند
        if (month === 12 && ValidationService.IsPersianLeapYear(year)) {
            maxDay = 30;
        }

        if (day > maxDay) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateDay2(fieldName));
            return false;
        }

        // بررسی زمان
        const timeParts = timePart.split(':');
        if (timeParts.length !== 3) return false;

        const hour = parseInt(timeParts[0], 10);
        const minute = parseInt(timeParts[1], 10);
        const second = parseInt(timeParts[2], 10);

        if (isNaN(hour)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateHourEmpty(filedName));
            return false;
        }

        if (isNaN(minute)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateMinuteEmpty(filedName));
            return false;
        }

        if (isNaN(second)) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateSecondEmpty(filedName));
            return false;
        }

        if (hour < 0 || hour > 23) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateHour(filedName));
            return false;
        }

        if (minute < 0 || minute > 59) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateMinute(filedName));
            return false;
        }

        if (second < 0 || second > 59) {
            ValidationService.ShowError(ValidationMessage.IsValidPersianDateSecond(fieldName));
            return false;
        }

        return true;
    },

    isValidIp(ip) {
        if (!ip || typeof ip !== 'string' || ip.trim() === '') {
            return false;
        }

        const ipPattern = /^(\d{1,3})\.(\d{1,3})\.(\d{1,3})\.(\d{1,3})$/;
        const match = ip.match(ipPattern);

        if (!match) {
            ValidationService.ShowError(ValidationMessage.isValidIp());
            return false;
        }

        // بررسی اینکه هر بخش بین 0 تا 255 باشد
        for (let i = 1; i <= 4; i++) {
            const part = parseInt(match[i], 10);
            if (part < 0 || part > 255) {
                ValidationService.ShowError(ValidationMessage.isValidIp());
                return false;
            }
        }

        return true;
    },

    isValidpassword(password) {

        const result = zxcvbn(password);
        const strength = result.score;

        if (strength < 3) {
            ValidationService.ShowError(ValidationMessage.isValidpassword());
            return false;
        }
        return true;
    },
















    ShowError: function (message) {
        NotyfNews(message, "error");
        //if (typeof notyfNews === "function") {
        //    NotyfNews(message, "error");
        //} else {
        //    console.error("notyfNews is not defined");
        //}
    },

    IsPersianLeapYear: function (year) {
        const a = year - (474);
        const b = a % 2820;
        return (((b + 474 + 38) * 682) % 2816) < 682;
    },

    getDaysInPersianMonth: function (year, month) {
        if (month <= 6) return 31;
        if (month <= 11) return 30;

        // سال کبیسه شمسی: هر 33 سال، تقریباً 8 کبیسه داریم (به‌طور ساده‌شده)
        return ValidationService.isPersianLeapYear(year) ? 30 : 29;
    },

    ConvertPersianDigitsToEnglish: function (input) {
        return input.replace(/[۰-۹]/g, d => String('۰۱۲۳۴۵۶۷۸۹'.indexOf(d)));
    }

};

window.ValidationService = ValidationService;
