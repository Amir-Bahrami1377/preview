using FormerUrban_Afta.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.Areas.Setting.Controllers;

[Area("Setting")]
public class AuditFilterController : Controller
{

    private readonly IAuditFilterService _auditFilterService;
    private readonly IHistoryLogService _historyLogService;

    public AuditFilterController(IAuditFilterService auditFilterService, IHistoryLogService historyLogService)
    {
        _auditFilterService = auditFilterService;
        _historyLogService = historyLogService;
    }


    [CheckUserAccess(permissionCode: "Menu_AuditFilters", type: EnumOperation.Get, table: EnumFormName.AuditFilters, section: "مدیریت فیلتر تغییرات")]
    public async Task<IActionResult> Index()
    {
        var model = await _auditFilterService.GetAllAsync();
        _historyLogService.PrepareForInsert(description: $"نمایش اطلاعات مدیریت فیلتر تغییرات", EnumFormName.AuditFilters, EnumOperation.Get);
        var valid = model.Where(x => !x.IsValid).ToList();
        foreach (var item in valid)
        {
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده مدیریت فیلتر تغییرات جدول {item.TableName}", EnumFormName.AuditFilters, EnumOperation.Validate);
        }
        return View(model: model);
    }

    [CheckUserAccess(permissionCode: "AuditFilters_Create", type: EnumOperation.Get, table: EnumFormName.AuditFilters, section: "مدیریت فیلتر تغییرات")]
    public async Task<IActionResult> Create()
    {
        var model = new AuditFilterDto
        {
            FormNameDrp = new SelectList(_auditFilterService.GetEnumFormNameInfo(), "Name", "DisplayName")
        };
        _historyLogService.PrepareForInsert(description: $"نمایش اطلاعات ایجاد فیلتر تغییرات", EnumFormName.AuditFilters, EnumOperation.Get);
        return PartialView(model);
    }

    [CheckUserAccess(permissionCode: "AuditFilters_Create", type: EnumOperation.Post, table: EnumFormName.AuditFilters, section: "مدیریت فیلتر تغییرات")]
    public async Task<IActionResult> CreateSubmit(AuditFilterDto command)
    {
        var result = await _auditFilterService.CreateAsync(command);
        _historyLogService.PrepareForInsert(description: result.Success ? $"افزودن فیلتر تغییرات فرم {result.Data}" : $"خطا در افزودن فیلتر تغییرات فرم{result.Data}", EnumFormName.AuditFilters, EnumOperation.Post);
        return new JsonResult(new { success = result.Success, message = result.Message });
    }

    [CheckUserAccess(permissionCode: "AuditFilters_Delete", type: EnumOperation.Delete, table: EnumFormName.AuditFilters, section: "مدیریت فیلتر تغییرات")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _auditFilterService.DeleteById(id);
        _historyLogService.PrepareForInsert(description: result.Success ? $"حذف فیلتر تغییرات فرم {result.Data}" : $"خطا در حذف اطلاعات فیلتر تغییرات فرم {result.Data}", EnumFormName.AuditFilters, EnumOperation.Delete);
        return new JsonResult(new { success = result.Success, message = result.Message });
    }
}
