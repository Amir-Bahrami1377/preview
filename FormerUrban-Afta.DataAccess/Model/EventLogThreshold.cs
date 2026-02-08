using FormerUrban_Afta.DataAccess.Model.BaseEntity;

namespace FormerUrban_Afta.DataAccess.Model;
public enum ThresholdSmsSendStatus
{
    [Display(Name = "ارسال شود")]
    Sent = 2,

    [Display(Name = "ارسال نشود")]
    NotSent = 1,

    [Display(Name = "نامشخص")]
    NotSpecific = 0
}

public class EventLogThreshold : BaseModel
{
    public string UsersLoginLogWarning { get; set; } = "1000";
    public string UsersLoginLogCritical { get; set; } = "1100";
    public string UsersActivityLogWarning { get; set; } = "1000";
    public string UsersActivityLogCritical { get; set; } = "1100";
    public string UsersAuditsLogWarning { get; set; } = "1000";
    public string UsersAuditsLogCritical { get; set; } = "1100";
    public string? UserId { get; set; }
    public string? UserName { get; set; }
    public ThresholdSmsSendStatus IsUserLoginLogWarningSmsSent { get; set; } = ThresholdSmsSendStatus.NotSent;
    public ThresholdSmsSendStatus IsUserActivityLogWarningSmsSent { get; set; } = ThresholdSmsSendStatus.NotSent;
    public ThresholdSmsSendStatus IsAuditsLogWarningSmsSent { get; set; } = ThresholdSmsSendStatus.NotSent;


    public override string ToString() => string.Join("", UsersLoginLogWarning, UsersLoginLogCritical, UsersActivityLogWarning,
        UsersActivityLogCritical, UserId, UserName, IsUserLoginLogWarningSmsSent, IsUserActivityLogWarningSmsSent, UsersAuditsLogWarning,
        UsersAuditsLogCritical, IsAuditsLogWarningSmsSent);

}

