using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Reports;

namespace FormerUrban_Afta.Areas.Reports.Controllers
{
    [Area("Reports")]
    public class AuditController : Controller
    {
        private readonly IAuditService _auditService;
        private readonly IHistoryLogService _historyLogService;

        public AuditController(IAuditService auditService, IHistoryLogService historyLogService)
        {
            _auditService = auditService;
            _historyLogService = historyLogService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CheckUserAccess(permissionCode: "Reports_Audit", type: EnumOperation.Get, table: EnumFormName.Audit, section: "گزارشات تغییرات")]
        public async Task<IActionResult> Index(AuditSearchDto search)
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

            if (!ValidatorService.SanitizeAndValidateInput(search.EntityId) && !string.IsNullOrWhiteSpace(search.EntityId))
            {
                search.EntityId = "";
                ViewBag.ErrorMessage = ValidationMessage.SanitizeInput("شناسه گزارش");
            }

            var model = new AuditViewDto
            {
                Audit = await _auditService.GetAllAsync(search),
                Search = await _auditService.GetDrp(search),
            };

            var valid = model.Audit.Where(x => !x.IsValid).ToList();
            if (valid.Count > 0)
                foreach (var item in valid)
                {
                    _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول تغییرات برای فرم {item.TableName}", EnumFormName.Audit, EnumOperation.Validate);
                }
            _historyLogService.PrepareForInsert($"نمایش گزارش تغییرات", EnumFormName.Audit, EnumOperation.Get);
            return View(model);
        }
    }
}
