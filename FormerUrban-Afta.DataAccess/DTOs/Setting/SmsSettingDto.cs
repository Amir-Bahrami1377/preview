namespace FormerUrban_Afta.DataAccess.DTOs.Setting
{
    public class SmsSettingDto
    {
        public string sms_user { get; set; }
        public string sms_pass { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string>? message { get; set; }
    }
}
