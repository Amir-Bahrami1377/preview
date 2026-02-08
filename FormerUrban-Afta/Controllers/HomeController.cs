namespace FormerUrban_Afta.Controllers
{
    public class HomeController : AmardBaseController
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<CostumIdentityUser> _userManager;
        public List<CostumIdentityUser> Users { get; set; } = [];
        private readonly RoleManager<CostumIdentityRole> _roleManager;
        private readonly IAuthService _authService;
        private readonly IErjaService _ecrjaService;
        private readonly IEventLogThresholdService _eventLogThresholdService;
        private readonly IPermissionService _permissionService;
        private readonly IUserLoginedService _userLoginedService;

        public HomeController(ILogger<HomeController> logger, UserManager<CostumIdentityUser> userManager,
            RoleManager<CostumIdentityRole> roleManager, IAuthService authService, IErjaService ecrjaService,
            IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService, IPermissionService permissionService, IUserLoginedService userLoginedService)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
            _ecrjaService = ecrjaService;
            _eventLogThresholdService = eventLogThresholdService;
            _permissionService = permissionService;
            _userLoginedService = userLoginedService;
        }

        public async Task<IActionResult> Index()
        {
            if (TempData.ContainsKey("FromLogin") && (bool)TempData["FromLogin"])
            {
                TempData["LoginMessage"] = "به نرم افزار شهرسازی آمارد خوش آمدید!";
            }

            var user = _authService.GetCurrentUser();
            var model = new List<KartableDTO>();
            if (await _permissionService.HasPermissionAsync(user.Id, "Menu_Kartabl"))
            {
                model = _ecrjaService.GetKartable();
                model.ForEach(item =>
                    item.ControllerName = item.c_marhaleh switch
                    {
                        1 => "Visit",
                        2 => "ControlMap",
                        3 => "Estelam",
                        4 => "Parvaneh",
                        _ => ""
                    });
            }

            var successfulLogin = await _userLoginedService.GetSuccessFullLogin();
            var loginFailed = await _userLoginedService.GetLoginFailed();

            var result = new HomeDto()
            {
                kartable = model,
                Success = successfulLogin,
                Failed = loginFailed
            };

            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ExceptionError()
        {

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Message = "خطایی رخ داده است لطفا با پشتیبانی تماس بگیرید."
            });
        }

    }
}
