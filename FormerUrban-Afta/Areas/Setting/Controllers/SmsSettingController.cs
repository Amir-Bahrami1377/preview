using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Setting;
using System.Threading.Tasks;

namespace FormerUrban_Afta.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class SmsSettingController : AmardBaseController
    {
        private readonly ITarifhaService _tarifhaService;
        private readonly IValidator<SmsSettingDto> _validator;
        private readonly IHistoryLogService _historyLogService;
        private readonly IEventLogThresholdService _eventLogThresholdService;

        public SmsSettingController(ITarifhaService tarifhaService, IValidator<SmsSettingDto> validator, IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService)
        {
            _tarifhaService = tarifhaService;
            _validator = validator;
            _historyLogService = historyLogService;
            _eventLogThresholdService = eventLogThresholdService;
        }

        [CheckUserAccess(permissionCode: "Menu_SmsSetting", type: EnumOperation.Get, table: EnumFormName.Tarifha, section: "مدیریت پنل پیامکی")]
        public async Task<IActionResult> Index()
        {
            var model = await _tarifhaService.GetSmsSetting();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "SmsSetting_Edit", type: EnumOperation.Update, table: EnumFormName.Tarifha, section: "ثبت مدیریت پنل پیامکی")]
        public async Task<IActionResult> Update(SmsSettingDto command)
        {
            var result = _validator.Validate(command);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا در ویرایش پنل پیامکی", EnumFormName.Tarifha, EnumOperation.Update);
                command.message = result.Errors.Select(e => e.ErrorMessage).ToList();
                return View("Index", command);
            }

            await _tarifhaService.UpdateSmsSetting(command);
            TempData["SuccessMessage"] = $"ویرایش پنل پیامکی با موفقیت انجام شد.";
            return RedirectToAction("Index");
        }
    }
}
