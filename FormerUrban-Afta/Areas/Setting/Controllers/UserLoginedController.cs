using FormerUrban_Afta.Attributes;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Setting.Controllers
{
    [Area("setting")]
    public class UserLoginedController : AmardBaseController
    {
        private readonly IUserLoginedService _userLogined;
        private readonly ILogger<UserLoginedController> _logger;
        private IAuthService _authenticateService;
        private readonly IValidator<UserLoginedSearchDto> _validator;
        private readonly IHistoryLogService _historyLogService;

        public UserLoginedController(IUserLoginedService userLogined, ILogger<UserLoginedController> logger, IAuthService authenticateService, IValidator<UserLoginedSearchDto> validator, IHistoryLogService historyLogService)
        {
            _userLogined = userLogined;
            _logger = logger;
            _authenticateService = authenticateService;
            _validator = validator;
            _historyLogService = historyLogService;
        }

        [CheckUserAccess(permissionCode: "Menu_UserLogined", type: EnumOperation.Get, table: EnumFormName.UserLogined, section: "لاگین کاربران")]
        public IActionResult Index(string? userName, string? ip, string? fromDateTime, string? toDateTime, string? arrivalDate, string? departureDate, int reportType)
        {
            var search = new UserLoginedSearchDto
            {
                UserName = userName,
                Ip = ip,
                FromDateTime = fromDateTime,
                ToDateTime = toDateTime,
                ArrivalDate = arrivalDate,
                DepartureDate = departureDate,
                ReportType = reportType,
            };

            ValidationResult result = _validator.Validate(search);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در عملیات جستجو لاگین کاربران.", EnumFormName.UserLogined, EnumOperation.Get);
                TempData["ValidateMessage"] = string.Join(" / ", result.Errors.Select(e => e.ErrorMessage));
                return View("Index", new List<UserLoginedDto>());
            }
            var model = _userLogined.Search(search);
            return View(model);
        }
    }
}
