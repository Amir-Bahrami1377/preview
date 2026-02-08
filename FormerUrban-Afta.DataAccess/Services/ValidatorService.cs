using System.Globalization;

namespace FormerUrban_Afta.DataAccess.Services
{
    public static class ValidatorService
    {
        // Constants for magic numbers and regex patterns
        private const int NationalCodeLength = 10;
        private const int LegalNationalIdLength = 11;
        private const int MinPersianYear = 1300;
        private const int MaxPersianYear = 1499;
        private const int MinPersianMonth = 1;
        private const int MaxPersianMonth = 12;
        private const int MinPersianDay = 1;
        private const int MaxPersianDay = 31;
        private const string NationalCodeRegex = @"^\d{10}$";
        private const string LegalNationalIdRegex = @"^\d{11}$";
        private const string TelRegex = @"^(0[1-9]{2,3})?\d{8}$";
        private const string PostalCodeRegex = @"^\d{10}$";
        private const string MobileRegex = @"^09\d{9}$";
        private const string InsuranceNumberRegex = @"^\d{8}$";
        private const string ScriptOrHtmlPattern = @"<script[\s\S]*?>|</script>|<.*?>";
        private const string UrlPattern = @"(http|https|ftp|www)\S*";
        private const string SqlPattern = @"\b(SELECT|INSERT|DELETE|UPDATE|DROP|ALTER|CREATE|EXEC|UNION|CAST|DECLARE|TRUNCATE|MERGE|GRANT|REVOKE|COMMIT|ROLLBACK|SAVEPOINT|SHUTDOWN)\b";
        private const string SpecialCharsPattern = @"(--|\bOR\b|\bAND\b|;|'|\(|\))";

        public static bool IsDigitsOnly(string input) => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[0-9]*$");
        public static bool IsValidCodeNosazi(string value) => Regex.IsMatch(value, @"^\d+(-\d+){6}$");
        public static bool Length(string value, int max) => value.Length == max;
        public static bool MaxLength(string value, int max) => value.Length < max;
        public static bool MinLength(string value, int min) => value.Length > min;
        public static bool IsPersianLetters(string input) => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[\u0600-\u06FF\s]*$");
        public static bool IsAlphanumeric(string input) => !string.IsNullOrWhiteSpace(input) && Regex.IsMatch(input, @"^[a-zA-Z0-9\u0600-\u06FF ,./\-]*$");
        public static bool IsValidTel(string? phoneNumber) => Regex.IsMatch(phoneNumber, TelRegex);
        public static bool IsValidPostalCode(string? postalCode) => Regex.IsMatch(postalCode, PostalCodeRegex);
        public static bool AmountIsValidFormat(double amount) => Regex.IsMatch(amount.ToString(), @"^\d+(\/\d{1,2})?$");
        public static bool AmountIsValidFormat(double? amount)
        {
            var str = amount?.ToString() ?? string.Empty;
            return Regex.IsMatch(str, @"^\d+(\/\d{1,2})?$", RegexOptions.Compiled);
        }
        public static bool IsValidUtmX(int x) => x is >= 200000 and <= 900000;
        public static bool IsValidUtmY(int y) => y is >= 3300000 and <= 4200000;
        public static bool IsValidMobileNumber(string mobile) => Regex.IsMatch(mobile, MobileRegex);
        public static bool IsValidInsuranceNumber(string insuranceNumber) => Regex.IsMatch(insuranceNumber, InsuranceNumberRegex);

        public static bool SanitizeAndValidateInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            var model = input.Trim();
            var dangerousChars = new[] { "<", ">", "\"", "'", ";", "%", "(", ")", "+", "=", "\\", "--", "`", "/" };

            foreach (var ch in dangerousChars)
                input = input.Replace(ch, string.Empty);

            input = Regex.Replace(input, @"\s{2,}", " ").Trim();
            input = Regex.Replace(input, @"[\x00-\x1F\x7F]", string.Empty);

            if (Regex.IsMatch(input, ScriptOrHtmlPattern, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, UrlPattern, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, SqlPattern, RegexOptions.IgnoreCase) ||
                Regex.IsMatch(input, SpecialCharsPattern, RegexOptions.IgnoreCase))
            {
                return false;
            }

            return model == input;
        }

        public static bool IsValidNationalCode(string nationalCode)
        {
            if (string.IsNullOrWhiteSpace(nationalCode))
                return true;

            if (!Regex.IsMatch(nationalCode, NationalCodeRegex))
                return false;

            if (new string(nationalCode[0], NationalCodeLength) == nationalCode)
                return false;

            var check = int.Parse(nationalCode[9].ToString());
            var sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(nationalCode[i].ToString()) * (10 - i);

            var remainder = sum % 11;

            return (remainder < 2 && check == remainder) || (remainder >= 2 && check == 11 - remainder);
        }

        public static bool IsValidLegalNationalId(string nationalId)
        {
            if (string.IsNullOrWhiteSpace(nationalId))
                return true;

            if (nationalId.Length != LegalNationalIdLength || !Regex.IsMatch(nationalId, LegalNationalIdRegex))
                return false;

            if (nationalId.Distinct().Count() == 1)
                return false;

            return true;
        }

        public static bool IsValidBirthCertificateNumber(string? number)
        {
            return Regex.IsMatch(number, @"^\d{1,10}$", RegexOptions.Compiled);
        }

        public static bool IsValidPersianDate(string? date)
        {
            if (string.IsNullOrWhiteSpace(date))
                return true;

            if (date.Length != 10)
                return false;

            date = ConvertPersianDigitsToEnglish(date ?? string.Empty);
            var parts = date.Split('/');

            if (parts.Length != 3 ||
                !int.TryParse(parts[0], out int year) ||
                !int.TryParse(parts[1], out int month) ||
                !int.TryParse(parts[2], out int day))
                return false;

            PersianCalendar pc = new();
            return year is >= MinPersianYear and <= MaxPersianYear &&
                   month is >= MinPersianMonth and <= MaxPersianMonth &&
                   day is >= MinPersianDay && day <= pc.GetDaysInMonth(year, month);
        }

        public static bool IsValidIpRangeOrCidr(string input)
        {
            var ipSegment = @"(?:\d{1,3})";
            var ipPattern = $@"^{ipSegment}\.{ipSegment}\.{ipSegment}\.{ipSegment}$";

            bool IsValidIp(string ip)
            {
                if (!Regex.IsMatch(ip, ipPattern)) return false;
                var parts = ip.Split('.').Select(int.Parse).ToArray();
                return parts.All(p => p >= 0 && p <= 255);
            }

            if (input.Contains("/"))
            {
                var parts = input.Split('/');
                if (parts.Length == 2 && IsValidIp(parts[0]) && int.TryParse(parts[1], out var cidr) && cidr is > 0 and <= 255)
                    return true;
                return false;
            }

            if (input.Contains("-"))
            {
                var parts = input.Split('-');
                if (parts.Length == 2 && IsValidIp(parts[0]) && IsValidIp(parts[1]))
                    return true;
                return false;
            }

            return IsValidIp(input);
        }

        public static bool IsValidIp(string? ip)
        {
            if (string.IsNullOrWhiteSpace(ip))
                return false;

            var ipSegment = @"(?:\d{1,3})";
            var ipPattern = $@"^{ipSegment}\.{ipSegment}\.{ipSegment}\.{ipSegment}$";

            if (!Regex.IsMatch(ip, ipPattern))
                return false;

            var parts = ip.Split('.');
            foreach (var part in parts)
            {
                if (!int.TryParse(part, out int num) || num < 0 || num > 255)
                    return false;
            }

            return true;
        }

        public static DateTime? ParsePersianDateTime(string? input)
        {
            if (!IsValidPersianDateTime(input))
                return null;

            input = ConvertPersianDigitsToEnglish(input ?? string.Empty);
            var parts = input.Split(' ');

            string? datePart = null;
            string? timePart = null;

            if (parts[0].Contains("/") && parts[1].Contains(":"))
            {
                datePart = parts[0];
                timePart = parts[1];
            }
            else if (parts[0].Contains(":") && parts[1].Contains("/"))
            {
                datePart = parts[1];
                timePart = parts[0];
            }
            else
            {
                return null;
            }

            var dateParts = datePart.Split('/');
            var timeParts = timePart.Split(':');

            if (!int.TryParse(dateParts[0], out int year) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int day) ||
                !int.TryParse(timeParts[0], out int hour) ||
                !int.TryParse(timeParts[1], out int minute) ||
                !int.TryParse(timeParts[2], out int second))
                return null;

            var pc = new PersianCalendar();
            try
            {
                return pc.ToDateTime(year, month, day, hour, minute, second, 0);
            }
            catch
            {
                return null;
            }
        }

        public static bool IsValidPersianDateTime(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return true;

            input = ConvertPersianDigitsToEnglish(input ?? string.Empty);
            var parts = input.Split(' ');

            if (parts.Length != 2)
                return false;

            string? datePart = null;
            string? timePart = null;

            if (parts[0].Contains("/") && parts[1].Contains(":"))
            {
                datePart = parts[0];
                timePart = parts[1];
            }
            else if (parts[0].Contains(":") && parts[1].Contains("/"))
            {
                datePart = parts[1];
                timePart = parts[0];
            }
            else
            {
                return false;
            }

            var dateParts = datePart.Split('/');
            if (dateParts.Length != 3 ||
                !int.TryParse(dateParts[0], out int year) ||
                !int.TryParse(dateParts[1], out int month) ||
                !int.TryParse(dateParts[2], out int day))
                return false;

            PersianCalendar pc = new();
            if (year < MinPersianYear || year > MaxPersianYear ||
                month < MinPersianMonth || month > MaxPersianMonth ||
                day < MinPersianDay || day > pc.GetDaysInMonth(year, month))
                return false;

            var timeParts = timePart.Split(':');
            if (timeParts.Length != 3 ||
                !int.TryParse(timeParts[0], out int hour) ||
                !int.TryParse(timeParts[1], out int minute) ||
                !int.TryParse(timeParts[2], out int second))
                return false;

            if (hour < 0 || hour > 23 ||
                minute < 0 || minute > 59 ||
                second < 0 || second > 59)
                return false;

            return true;
        }

        private static string ConvertPersianDigitsToEnglish(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            return input
                .Replace('۰', '0').Replace('۱', '1').Replace('۲', '2')
                .Replace('۳', '3').Replace('۴', '4').Replace('۵', '5')
                .Replace('۶', '6').Replace('۷', '7').Replace('۸', '8')
                .Replace('۹', '9');
        }
    }
}
