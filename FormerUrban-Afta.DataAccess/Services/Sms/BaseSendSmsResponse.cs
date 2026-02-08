namespace FormerUrban_Afta.DataAccess.Services.Sms
{
    public class BaseSendSmsResponse
    {
        #region Props
        public bool IsOk { get; private set; }
        public string SentContext { get; private set; }
        public string RecievedContext { get; private set; }

        public BaseSendSmsResponse() : this(false, string.Empty) { }

        public BaseSendSmsResponse(bool isOk, string recievedContext, string sentContext = null)
        {
            IsOk = isOk;
            SentContext = sentContext;
            RecievedContext = recievedContext;
        }
        #endregion

        public override string ToString() => this is null ? string.Empty : IsOk ? $"ارسال موفق - متن پیام: {SentContext}" : $"ارسال ناموفق - متن پیام: {SentContext} - متن خطا: {RecievedContext}";
    }
}
