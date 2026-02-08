namespace FormerUrban_Afta.DataAccess.DTOs.Setting
{
    public class LoginSettingDto
    {
        public string? RetryLoginCount { get; set; }
        public string? MaximumSessions { get; set; }
        public string? KhatemeSessionAfterMinute { get; set; }
        public string? MaximumAccessDenied { get; set; }
        public string? RequestRateLimitter { get; set; }
        public string? PasswordLength { get; set; }
        public string? UnblockingUserTime { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string>? message { get; set; }

        //public string sms_user { get; set; }
        //public string sms_pass { get; set; }
        //public string sms_shomare { get; set; }
    }
}
