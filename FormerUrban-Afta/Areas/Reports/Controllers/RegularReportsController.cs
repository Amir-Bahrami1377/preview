using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Reports;

namespace FormerUrban_Afta.Areas.Reports.Controllers;

[Area("Reports")]

public class RegularReportsController : AmardBaseController
{

    private readonly IHistoryLogService _historyLogService;

    public RegularReportsController(IHistoryLogService historyLogService)
    {
        _historyLogService = historyLogService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Reports_Regular", type: EnumOperation.Get, table: EnumFormName.History, section: "گزارشات عادی")]
    public async Task<IActionResult> Index(SearchHistoryDto search)
    {
        if (!ValidatorService.IsValidPersianDate(search.Date))
        {
            search.Date = "";
            ViewBag.ErrorMessage = ValidationMessage.IsValidPersianDate("تاریخ");
        }

        if (!ValidatorService.IsValidIp(search.Ip) && !string.IsNullOrWhiteSpace(search.Ip))
        {
            search.Ip = "";
            ViewBag.ErrorMessage = ValidationMessage.IsValidIpRangeOrCidr();
        }

        var model = new HistoryViewDto
        {
            HistoryDtos = await _historyLogService.SearchAsync(search),
            SearchHistoryDto = await _historyLogService.GetDrp(search),
        };
        var valid = model.HistoryDtos.Where(x => !x.IsValid).ToList();
        if (valid.Count > 0)
            foreach (var item in valid)
            {
                _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول لاگ عادی با آیدی {item.Identity}", EnumFormName.History, EnumOperation.Validate);
            }
        _historyLogService.PrepareForInsert($"نمایش گزارشات عادی", EnumFormName.History, EnumOperation.Get);
        return View(model);
    }
}

