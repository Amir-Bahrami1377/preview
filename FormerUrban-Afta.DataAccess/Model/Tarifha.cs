using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model
{
    public class Tarifha : BaseModel
    {

        // Sms
        public string sms_user { get; set; } = "09212364470";
        public string sms_pass { get; set; } = "tk@LNRYxqeaPfe8U";


        // Login Setting
        public string? RetryLoginCount { get; set; } = "3";
        public string? MaximumSessions { get; set; } = "3";
        public string? KhatemeSessionAfterMinute { get; set; } = "5";
        public string? MaximumAccessDenied { get; set; } = "3";
        public string? RequestRateLimitter { get; set; } = "90";
        public string? UnblockingUserTime { get; set; } = "1";

        // Password
        public string? PasswordLength { get; set; } = "12";


        public override string ToString()
        {
            return string.Join("", sms_user?.Trim(), sms_pass?.Trim(), UnblockingUserTime?.Trim(), PasswordLength, RetryLoginCount, MaximumSessions, MaximumAccessDenied,
                KhatemeSessionAfterMinute, RequestRateLimitter, CreateDateTime.Ticks, ModifiedDate?.Ticks ?? 0, CreateUser, ModifiedUser).Trim();
        }

    }
}
