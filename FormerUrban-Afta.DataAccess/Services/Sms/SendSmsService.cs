using FormerUrban_Afta.DataAccess.Model.SMS;

namespace FormerUrban_Afta.DataAccess.Services.Sms;
public class SendSmsService : ISendSmsService
{
    private readonly MelipayamakSmsService _melipayamakSmsService;
    private readonly IAuthService _authenticateService;

    public SendSmsService(MelipayamakSmsService melipayamakSmsService, IAuthService authenticateService)
    {
        _melipayamakSmsService = melipayamakSmsService;
        _authenticateService = authenticateService;
    }

    public async Task<MeliPayamakRestResponse> SendMessageSmsWithRespondToSuperusers(string message, string userId)
    {
        var user = await _authenticateService.GetAsync(userId);
        if (user == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
            return new MeliPayamakRestResponse();

        return await _melipayamakSmsService.SendSms(message, user.PhoneNumber);
    }

    public async Task<string> SendMessageSmsWithRespondToSuperusers(string message, string logMessage, string userId, int bodyId)
    {
        var user = await _authenticateService.GetAsync(userId);
        if (user.UserName == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
            user = await _authenticateService.GetByUserNameAsync("developer");

        if (user.UserName == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
            return user?.PhoneNumber ?? "";

        await _melipayamakSmsService.SendSms(message, logMessage, user.PhoneNumber, bodyId);

        return user.PhoneNumber;
    }

    public async Task<MeliPayamakRestResponse> SendMessageSmsWithRespondToSuperusers2(string message, string logMessage, string userId, int bodyId)
    {
        var user = await _authenticateService.GetAsync(userId);
        if (user.UserName == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
            user = await _authenticateService.GetByUserNameAsync("developer");

        //if (user.UserName == null || string.IsNullOrWhiteSpace(user.PhoneNumber))
        //    return user?.PhoneNumber ?? "";

        return await _melipayamakSmsService.SendSms(message, logMessage, user.PhoneNumber ?? "", bodyId);
    }
}
