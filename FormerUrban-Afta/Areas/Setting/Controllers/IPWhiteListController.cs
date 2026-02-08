using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Setting.Controllers
{
    [Area("Setting")]
    public class IPWhiteListController : AmardBaseController
    {
        private readonly IEventLogThresholdService _eventLogThresholdService;
        private readonly IHistoryLogService _historyLogService;
        private readonly IValidator<AllowedIPRangeDto> _allowedIPRangeValidator;
        private readonly IAllowedIPRange _allowedIpRangeService;
        public IPWhiteListController(IAllowedIPRange allowedIpRangeService,
            IHistoryLogService historyLogService, IEventLogThresholdService eventLogThreshold, IValidator<AllowedIPRangeDto> allowedIpRangeValidator)
        {
            _allowedIpRangeService = allowedIpRangeService;
            _historyLogService = historyLogService;
            _eventLogThresholdService = eventLogThreshold;
            _allowedIPRangeValidator = allowedIpRangeValidator;
        }

        [CheckUserAccess(permissionCode: "Menu_IPWhiteList", type: EnumOperation.Get, table: EnumFormName.AllowedIPRange, section: "مدیریت IP مجاز")]
        public async Task<IActionResult> Index()
        {
            var data = await _allowedIpRangeService.GetAllAllowedIPRangeAsync();
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "IpWhite_Create", type: EnumOperation.Get, table: EnumFormName.AllowedIPRange, section: "مشاهده ایجاد IP مجاز")]
        public async Task<IActionResult> CreateWhiteIp()
        {
            _historyLogService.PrepareForInsert($"مشاهده ایجاد IP مجاز", EnumFormName.AllowedIPRange, EnumOperation.Get);
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "IpWhite_Create", type: EnumOperation.Post, table: EnumFormName.AllowedIPRange, section: "ایجاد IP مجاز")]
        public async Task<IActionResult> CreateWhiteIpSubmit(AllowedIPRangeDto obj)
        {
            var result = await _allowedIPRangeValidator.ValidateAsync(obj);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا صحت سنجی در ایجاد آیپی های وایت لیست", EnumFormName.AllowedIPRange, EnumOperation.Validate);
                return new JsonResult(new { success = false, message = result.Errors.Select(e => e.ErrorMessage).ToList() });
            }

            var res = await _allowedIpRangeService.AddAllowedIPRangeAsync(obj);
            var message = res ? $"رنج آی پی {obj.IPRange} با موفقیت مجاز شد." : "انجام عملیات ناموفق بود لطفا مجددا تلاش کنید!";
            if (res)
                TempData["SuccessMessage"] = $"رنج آی پی {obj.IPRange} با موفقیت مجاز شد.";
            return new JsonResult(new { success = res, message = message });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "IpWhite_Edit", type: EnumOperation.Get, table: EnumFormName.AllowedIPRange, section: "مشاهده ویرایش IP مجاز")]
        public async Task<IActionResult> EditWhiteIp(int identity)
        {
            var data = await _allowedIpRangeService.GetAllowedIPRangeAsync(identity);
            return PartialView(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "IpWhite_Edit", type: EnumOperation.Update, table: EnumFormName.AllowedIPRange, section: "ویرایش IP مجاز")]
        public async Task<IActionResult> EditWhiteIpSubmit(AllowedIPRangeDto obj)
        {
            var result = await _allowedIPRangeValidator.ValidateAsync(obj);
            if (!result.IsValid)
            {
                _historyLogService.PrepareForInsert($"خطا صحت سنجی در ویرایش وایت لیست {obj.Identity}", EnumFormName.AllowedIPRange, EnumOperation.Validate);
                return new JsonResult(new { success = false, message = result.Errors.Select(e => e.ErrorMessage).ToList() });
            }

            var res = await _allowedIpRangeService.UpdateAllowedIPRangeAsync(obj);
            var message = res ? $"ویرایش رنج آی پی {obj.IPRange} با موفقیت انجام شد." : "انجام عملیات ناموفق بود لطفا مجددا تلاش کنید!";
            if (res)
                TempData["SuccessMessage"] = $"ویرایش رنج آی پی {obj.IPRange} با موفقیت مجاز شد.";
            return new JsonResult(new { success = true, message = message });
        }

        [HttpGet]
        [CheckUserAccess(permissionCode: "IpWhite_Delete", type: EnumOperation.Delete, table: EnumFormName.AllowedIPRange, section: "حذف IP مجاز")]
        public async Task<IActionResult> DeleteRow(long id)
        {
            var res = await _allowedIpRangeService.DeleteAllowedIPRangeAsync(id);
            if (res)
                TempData["SuccessMessage"] = $"رنج آی پی با شماره {id} با موفقیت حذف شد";
            return new JsonResult(new { success = true, message = res ? "با موفقیت حذف شد." : "حذف ناموفق بود." });
        }
    }
}
