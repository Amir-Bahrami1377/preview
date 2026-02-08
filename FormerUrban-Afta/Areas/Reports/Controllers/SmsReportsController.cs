using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Reports;

namespace FormerUrban_Afta.Areas.Reports.Controllers;

[Area("Reports")]

public class SmsReportsController : Controller
{
    private readonly ILogSMSService _logSmsService;
    private readonly IHistoryLogService _historyLogService;

    public SmsReportsController(ILogSMSService logSmsService, IHistoryLogService historyLogService)
    {
        _logSmsService = logSmsService;
        _historyLogService = historyLogService;
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Reports_Sms", type: EnumOperation.Get, table: EnumFormName.LogSMS, section: "گزارشات پیامک")]
    public async Task<IActionResult> Index(SearchLogSms search)
    {
        if (!ValidatorService.IsValidPersianDateTime(search.Date))
        {
            search.Date = "";
            ViewBag.ErrorMessage = ValidationMessage.IsValidPersianDate("تاریخ پیامک");
        }
        if (!string.IsNullOrWhiteSpace(search.Mobile))
        {
            if (!ValidatorService.IsDigitsOnly(search.Mobile))
                ViewBag.ErrorMessage = ValidationMessage.OnlyDigits("شماره موبایل");

            if (search.Mobile.Length > 12)
                ViewBag.ErrorMessage = ValidationMessage.MaxLength("شماره موبایل", 12);
        }

        var model = new LogSmsView
        {
            LogSmsDtos = await _logSmsService.SearchAsync(search),
            SearchLogSms = await _logSmsService.GetDrp(search),
        };

        var valid = model.LogSmsDtos.Where(x => !x.IsValid).ToList();
        if (valid.Count > 0)
            foreach (var item in valid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول لاگ پیامک با نام {item.FullName}", EnumFormName.LogSMS, EnumOperation.Validate);
            }
        _historyLogService.PrepareForInsert($"نمایش لاگ پیامک", EnumFormName.LogSMS, EnumOperation.Get);
        return View(model);
    }
}

