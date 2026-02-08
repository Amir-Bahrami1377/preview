using FormerUrban_Afta.Attributes;

namespace FormerUrban_Afta.Areas.Setting.Controllers;
[Area("Setting")]
public class IPBlockListController : Controller
{
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IValidator<BlockedIPRangeDto> _dtoValidator;
    private readonly IBlockedIPRange _blockedIpRangeService;
    private readonly ILogger<IPBlockListController> _logger;
    public IPBlockListController(IBlockedIPRange blockedIpRange, IValidator<BlockedIPRangeDto> blockedIpRangeDtoValidator,
        IHistoryLogService historyLogService, IEventLogThresholdService eventLogThreshold, ILogger<IPBlockListController> logger)
    {
        _eventLogThresholdService = eventLogThreshold;
        _historyLogService = historyLogService;
        _dtoValidator = blockedIpRangeDtoValidator;
        _blockedIpRangeService = blockedIpRange;
        _logger = logger;
    }

    [CheckUserAccess(permissionCode: "Menu_IPBlockList", type: EnumOperation.Get, table: EnumFormName.BlockedIPRange, section: "مدیریت IP مسدود شده")]
    public async Task<IActionResult> Index()
    {
        var data = await _blockedIpRangeService.GetAllBlockedIPRangeAsync();
        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "IpBlock_Create", type: EnumOperation.Get, table: EnumFormName.BlockedIPRange, section: "مشاهده ایجاد IP مسدود شده")]
    public async Task<IActionResult> CreateBlackIp()
    {
        _historyLogService.PrepareForInsert($"مشاهده ایجاد IP مسدود شده", EnumFormName.BlockedIPRange, EnumOperation.Get);
        return PartialView();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "IpBlock_Create", type: EnumOperation.Post, table: EnumFormName.BlockedIPRange, section: "ایجاد IP مسدود شده")]
    public async Task<IActionResult> CreateBlackIpSubmit(BlockedIPRangeDto obj)
    {
        var result = await _dtoValidator.ValidateAsync(obj);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ایجاد بلاک لیست", EnumFormName.BlockedIPRange, EnumOperation.Post);
            return new JsonResult(new { success = false, message = result.Errors.Select(e => e.ErrorMessage).ToList() });
        }

        var res = await _blockedIpRangeService.AddBlockedIPRangeAsync(obj);
        var message = res ? $"رنج آی پی {obj.IPRange} با موفقیت مسدود شد." : "انجام عملیات ناموفق بود لطفا مجددا تلاش کنید!";
        if (res)
            TempData["SuccessMessage"] = $"رنج آی پی {obj.IPRange} با موفقیت مسدود شد.";
        return new JsonResult(new { success = true, message = message });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "IpBlock_Edit", type: EnumOperation.Get, table: EnumFormName.BlockedIPRange, section: "مشاهده ویرایش IP مسدود شده")]
    public async Task<IActionResult> EditBlackIp(int identity)
    {
        var data = await _blockedIpRangeService.GetBlockedIPRangeAsync(identity);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "IpBlock_Edit", type: EnumOperation.Update, table: EnumFormName.BlockedIPRange, section: "ویرایش IP مسدود شده")]
    public async Task<IActionResult> EditBlackIpSubmit(BlockedIPRangeDto obj)
    {
        var result = await _dtoValidator.ValidateAsync(obj);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا صحت سنجی در ویرایش بلاک لیست {obj.IPRange}", EnumFormName.BlockedIPRange, EnumOperation.Post);
            return new JsonResult(new { success = false, message = result.Errors.Select(e => e.ErrorMessage).ToList() });
        }

        var res = await _blockedIpRangeService.UpdateBlockedIPRangeAsync(obj);
        var message = res ? $"ویرایش رنج آی پی {obj.IPRange} با موفقیت انجام شد." : "انجام عملیات ناموفق بود لطفا مجددا تلاش کنید!";
        if (res)
            TempData["SuccessMessage"] = $"ویرایش رنج آی پی {obj.IPRange} با موفقیت انجام شد.";
        return new JsonResult(new { success = true, message = message });
    }

    [HttpGet]
    [CheckUserAccess(permissionCode: "IpBlock_Delete", type: EnumOperation.Delete, table: EnumFormName.BlockedIPRange, section: "حذف IP مسدود شده")]
    public async Task<IActionResult> DeleteRow(long id)
    {
        var res = await _blockedIpRangeService.DeleteBlockedIPRangeAsync(id);
        if (res)
            TempData["SuccessMessage"] = $"با موفقیت حذف شد";
        return new JsonResult(new { success = true, message = res ? "با موفقیت حذف شد." : "حذف ناموفق بود." });
    }
}