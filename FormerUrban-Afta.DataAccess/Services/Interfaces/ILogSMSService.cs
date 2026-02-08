using FormerUrban_Afta.DataAccess.DTOs.Reports;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface ILogSMSService
    {
        public Task<List<LogSmsDto>> SearchAsync(SearchLogSms search);
        public LogSMS Insert(string status, string usercode, string text, string mobile, bool doHistoryLog = true);
        public Task<SearchLogSms> GetDrp(SearchLogSms search);
    }
}
