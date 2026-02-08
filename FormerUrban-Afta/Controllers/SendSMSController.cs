namespace FormerUrban_Afta.Controllers
{
    public class SendSMSController : Controller
    {
        private readonly ILogger<SendSMSController> _logger;
        private readonly IAuthService _authService;
        private readonly IHistoryLogService _historyLogService;

        public SendSMSController(ILogger<SendSMSController> logger, IAuthService authService, IHistoryLogService historyLogService)
        {
            _logger = logger;
            _authService = authService;
            _historyLogService = historyLogService;
        }
    }
}
