using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class EventLogFilterController : Controller
    {
        private readonly IEventLogFilterService _eventLogFilterService;
        private readonly ILogger<EventLogFilterController> _logger;
        public EventLogFilterController(IEventLogFilterService eventLogFilterService, ILogger<EventLogFilterController> logger)
        {
            _eventLogFilterService = eventLogFilterService;
            _logger = logger;
        }

        [CheckUserAccess(permissionCode: "Menu_EventLogFilter", type: EnumOperation.Get, table: EnumFormName.EventLogFilter, section: "مدیریت فیلتر رویداد های ممیزی")]
        public IActionResult Index()
        {
            var model = _eventLogFilterService.Get();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "EventLogFilter_Edit", type: EnumOperation.Update, table: EnumFormName.EventLogFilter, section: "ویرایش اطلاعات مدیریت فیلتر رویداد های ممیزی")]
        public async Task<IActionResult> Update(EventLogFilterDto model)
        {
            var result = false;
            if (!await _eventLogFilterService.Exists())
                result = await _eventLogFilterService.Add(model);
            else
                result = await _eventLogFilterService.Update(model);

            if (!result)
                model.message = ["عملیات با خطا مواجه شد!"];

            TempData["SuccessMessage"] = "عملیات با موفقیت انجام شد.";
            return RedirectToAction("Index");
        }
    }
}
