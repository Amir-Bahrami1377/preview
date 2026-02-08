using System.ComponentModel;
using System.Reflection;

namespace FormerUrban_Afta.DataAccess.Utilities
{
    public static class MyFunction2
    {
        #region Enum

        public static string GetDisplayEnumName(this Enum enumValue)
        {
            var target = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            var description = target?.GetCustomAttribute<DisplayAttribute>()?.Name ?? "?";
            return description;
        }

        public static string GetDescriptionEnumName(this Enum enumValue)
        {
            var target = enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault();
            return target?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? "?";
        }

        public static string GetEnumDescription(Enum value)
        {
            var fi = value.GetType().GetField(value.ToString());

            if (fi.GetCustomAttributes(typeof(DescriptionAttribute), false) is DescriptionAttribute[] attributes && attributes.Any())
                return attributes.First().Description;

            return value.ToString();
        }

        public static List<T> GetEnamList<T>() => Enum.GetValues(typeof(T)).Cast<T>().Select(v => v).ToList();

        public static object GetValueFromName<T>(this string name) where T : Enum
        {
            var type = typeof(T);

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(DisplayAttribute)) is DisplayAttribute attribute)
                {
                    if (attribute.Name == name)
                        return (T)field.GetValue(null);
                }

                if (field.Name == name)
                    return (T)field.GetValue(null);
            }

            return null;
        }

        public static string GetDescriptionFromName<TEnum>(string name) where TEnum : struct, Enum
        {
            if (Enum.TryParse<TEnum>(name, out var enumValue))
                return GetDescription(enumValue);

            return name;
        }

        public static string GetDescription<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            FieldInfo field = typeof(TEnum).GetField(value.ToString());
            var attribute = field?.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }

        /// <summary>
        /// دریافت مقدار Display.Name از یک enum با استفاده از نوع enum و مقدار عددی
        /// </summary>
        /// <typeparam name="T">نوع enum (مثلاً EnumPermission)</typeparam>
        /// <param name="value">مقدار عددی enum (مثلاً 1)</param>
        /// <returns>مقدار Name از Display یا null اگر پیدا نشد</returns>
        public static string GetDisplayValue<T>(int value) where T : Enum
        {
            // بررسی می‌کنیم که آیا این عدد معتبر برای enum است
            if (!Enum.IsDefined(typeof(T), value))
                return null;

            // تبدیل عدد به مقدار enum
            var enumValue = (T)(object)value;

            // دریافت نام ممبر (مثلاً UserChangePassword)
            var memberInfo = typeof(T).GetMember(enumValue.ToString());
            if (memberInfo.Length > 0)
            {
                // دریافت صفت Display
                var displayAttr = memberInfo[0].GetCustomAttribute<DisplayAttribute>();
                if (displayAttr != null)
                    return displayAttr.Name;
            }

            // اگر Display وجود نداشت، خود نام مقدار enum برگردانده می‌شود
            return enumValue.ToString();
        }

        #endregion
    }
}
