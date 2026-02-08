namespace FormerUrban_Afta.DataAccess.Utilities
{
    public class ExtensionUser
    {
        private readonly ITarifhaService _tarifhaService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IWeakPasswordService _weakPasswordService;
        private readonly IEventLogFilterService _eventLogFilterService;

        public ExtensionUser(ITarifhaService tarifhaService, IHistoryLogService historyLogService, IWeakPasswordService weakPasswordService, IEventLogFilterService eventLogFilterService)
        {
            _tarifhaService = tarifhaService;
            _historyLogService = historyLogService;
            _weakPasswordService = weakPasswordService;
            _eventLogFilterService = eventLogFilterService;
        }

        public async Task<TaskResult> CheckPasswordValidation(string password, bool doSuppressExceptions = false)
        {
            if (string.IsNullOrWhiteSpace(password))
                return new TaskResult(isOk: false, "رمز عبور نمی‌تواند خالی باشد.");

            var tarifha = await _tarifhaService.GetTarifhaAsync();

            if (string.IsNullOrWhiteSpace(tarifha.PasswordLength))
                tarifha.PasswordLength = "12";
            else
            {
                if (Convert.ToInt32(tarifha.PasswordLength) < 12)
                    tarifha.PasswordLength = "12";

                if (Convert.ToInt32(tarifha.PasswordLength) > 128)
                    tarifha.PasswordLength = "128";
            }

            if (password.Length < Convert.ToInt32(tarifha.PasswordLength))
            {
                if (_eventLogFilterService.Get().LogBarayeRaddeRamzeObour)
                    _historyLogService.PrepareForInsert("رد درخواست تغییر رمزعبور بدلیل عدم رعایت حداقل طول در رمزعبور جدید", EnumFormName.AspNetUsers, EnumOperation.Update);
                return new TaskResult(isOk: false, $"حداقل طول رمز عبور باید {tarifha.PasswordLength} باشد.");
            }

            if (password.Length > (int)EnumPasswordCheck.MaxLength)
            {
                if (_eventLogFilterService.Get().LogBarayeRaddeRamzeObour)
                    _historyLogService.PrepareForInsert("رد درخواست تغییر رمزعبور بدلیل عدم رعایت حداکثر طول در رمزعبور جدید", EnumFormName.AspNetUsers, EnumOperation.Update);
                return new TaskResult(isOk: false, $"حداکثر طول رمز عبور باید {(int)EnumPasswordCheck.MaxLength} باشد.");
            }

            var passwordCheck = _weakPasswordService.Search(password);
            if (passwordCheck != null)
            {
                if (_eventLogFilterService.Get().LogBarayeRaddeRamzeObour)
                    _historyLogService.PrepareForInsert("رد درخواست تغییر رمزعبور بدلیل موجود بودن پسورد در لیست پسورد های شکسته شده.", EnumFormName.AspNetUsers, EnumOperation.Update);
                return new TaskResult(isOk: false, "رد درخواست تغییر رمزعبور بدلیل موجود بودن پسورد در لیست پسورد های شکسته شده");
            }

            return new TaskResult(isOk: true, string.Empty);
        }
    }
}