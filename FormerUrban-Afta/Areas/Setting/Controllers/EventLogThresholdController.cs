using FormerUrban_Afta.Attributes;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class EventLogThresholdController : Controller
    {
        private readonly IEventLogThresholdService _eventLogThresholdService;
        private readonly ILogger<EventLogThresholdController> _logger;
        private readonly IValidator<EventLogThresholdDto> _validator;
        private readonly IHistoryLogService _historyLogService;
        public EventLogThresholdController(IEventLogThresholdService eventLogThresholdService, ILogger<EventLogThresholdController> logger, IValidator<EventLogThresholdDto> validator, IHistoryLogService historyLogService)
        {
            _eventLogThresholdService = eventLogThresholdService;
            _logger = logger;
            _validator = validator;
            _historyLogService = historyLogService;
        }

        [CheckUserAccess(permissionCode: "Menu_EventLogThreshold", type: EnumOperation.Get, table: EnumFormName.EventLogThreshold, section: "مدیریت حد آستانه رویداد های ممیزی")]
        public async Task<IActionResult> Index()
        {
            var model = await _eventLogThresholdService.GetAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "EventLogThreshold_Edit", type: EnumOperation.Update, table: EnumFormName.EventLogThreshold, section: "ویرایش اطلاعات مدیریت حد آستانه رویداد های ممیزی")]
        public async Task<IActionResult> Update(EventLogThresholdDto model)
        {
            ValidationResult result = _validator.Validate(model);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در عملیات ثبت حد آستانه رویداد های ممیزی.", EnumFormName.EventLogThreshold, EnumOperation.Post);
                model.message = result.Errors.Select(e => e.ErrorMessage).ToList();
                model = await _eventLogThresholdService.GetUserDrp(model);
                return View("Index", model);
            }

            if (!await _eventLogThresholdService.ExistsAsync())
                await _eventLogThresholdService.AddAsync(model);
            else
                await _eventLogThresholdService.Update(model);

            _historyLogService.PrepareForInsert($"عملیات ثبت حد آستانه رویداد های ممیزی با موفقیت انجام شد.", EnumFormName.EventLogThreshold, EnumOperation.Post);
            TempData["SuccessMessage"] = "عملیات ثبت حد آستانه رویداد های ممیزی با موفقیت انجام شد.";
            return RedirectToAction("Index");
        }
    }
}
