using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.DataAccess.DTOs.Setting
{
    public class EventLogThresholdDto
    {
        public long Identity { get; set; }
        public string UsersLoginLogWarning { get; set; }
        public string UsersLoginLogCritical { get; set; }
        public string UsersActivityLogWarning { get; set; }
        public string UsersActivityLogCritical { get; set; }
        public string UsersAuditsLogWarning { get; set; }
        public string UsersAuditsLogCritical { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }

        public ThresholdSmsSendStatus IsUserLoginLogWarningSmsSent { get; set; }
        public ThresholdSmsSendStatus IsUserActivityLogWarningSmsSent { get; set; }
        public ThresholdSmsSendStatus IsAuditsLogWarningSmsSent { get; set; }

        public SelectList Users { get; set; }

        public bool IsValid { get; set; } = true;

        public List<string>? message { get; set; }
    }
}
