namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface ITarifhaService
    {
        public Task<Tarifha> GetTarifhaAsync();
        public Task<Tarifha> GetTarifhaNoLogAsync();

        // Sms
        public Task<SmsSettingDto> GetSmsSetting();
        public Task<bool> UpdateSmsSetting(SmsSettingDto command);

        // Login
        public Task<LoginSettingDto> GetLoginSetting();
        public Task<bool> UpdateLoginSetting(LoginSettingDto command);
    }
}
