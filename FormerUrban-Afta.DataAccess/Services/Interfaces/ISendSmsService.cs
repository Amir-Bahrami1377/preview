using FormerUrban_Afta.DataAccess.Model.SMS;

namespace FormerUrban_Afta.DataAccess.Services.Interfaces
{
    public interface ISendSmsService
    {
        public Task<MeliPayamakRestResponse> SendMessageSmsWithRespondToSuperusers(string message, string userId);
        public Task<string> SendMessageSmsWithRespondToSuperusers(string message, string logMessage, string userId, int bodyId);
        public Task<MeliPayamakRestResponse> SendMessageSmsWithRespondToSuperusers2(string message, string logMessage, string userId, int bodyId);
    }
}
