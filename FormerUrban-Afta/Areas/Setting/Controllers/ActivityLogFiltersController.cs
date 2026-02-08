using FormerUrban_Afta.Attributes;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.Areas.Setting.Controllers;

[Area("Setting")]
public class ActivityLogFiltersController : Controller
{
    private readonly IActivityLogFiltersService _activityLogFiltersService;
    private readonly IHistoryLogService _historyLogService;

    public ActivityLogFiltersController(IActivityLogFiltersService activityLogFiltersService, IHistoryLogService historyLogService)
    {
        _activityLogFiltersService = activityLogFiltersService;
        _historyLogService = historyLogService;
    }

    [CheckUserAccess(permissionCode: "Menu_ActivityLogFilters", type: EnumOperation.Get, table: EnumFormName.ActivityLogFilters, section: "گزارشات فعال")]
    public IActionResult Index()
    {
        var data = _activityLogFiltersService.GetAll();
        _historyLogService.PrepareForInsert($"نمایش اطلاعات گزارشات فعال", EnumFormName.ActivityLogFilters, EnumOperation.Get);
        var valid = data.Where(x => !x.IsValid).ToList();
        foreach (var item in valid)
        {
            _historyLogService.PrepareForInsert($"رد صحت سنجی داده جدول گزارشات فعال با آیدی {item.FormName}", EnumFormName.ActivityLogFilters, EnumOperation.Validate);
        }
        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "ActivityLogFilters_Create", type: EnumOperation.Get, table: EnumFormName.ActivityLogFilters, section: "نمایش ایجاد گزارش فعال")]
    public IActionResult Create()
    {
        var data = new ActivityLogFiltersDto
        {
            Tables = new SelectList(_activityLogFiltersService.GetEnumFormNameInfo(), "Name", "DisplayName")
        };
        _historyLogService.PrepareForInsert($"نمایش ایجاد اطلاعات گزارشات فعال", EnumFormName.ActivityLogFilters, EnumOperation.Get);

        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "ActivityLogFilters_Create", type: EnumOperation.Get, table: EnumFormName.ActivityLogFilters, section: "نمایش ایجاد گزارش فعال")]
    public async Task<IActionResult> CreateSubmit(ActivityLogFiltersDto command)
    {
        var res = await _activityLogFiltersService.Add(command);
        _historyLogService.PrepareForInsert(
            res.Success
                ? $"عملیات ثبت لاگ ها برای فرم {command.TableName} با موفقیت انجام شد."
                : $"عملیات ثبت لاگ ها برای فرم {command.TableName} با خطا مواجه شد.",
            EnumFormName.ActivityLogFilters,
            EnumOperation.Post);
        if (res.Success)
            TempData["SuccessMessage"] = $"عملیات ایجاد ثبت لاگ ها برای فرم {command.TableName} با موفقیت انجام شد.";

        return new JsonResult(res);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "ActivityLogFilters_Edit", type: EnumOperation.Get, table: EnumFormName.ActivityLogFilters, section: "نمایش ایجاد گزارش فعال")]
    public async Task<IActionResult> Edit(long identity)
    {
        var data = _activityLogFiltersService.GetById(identity);
        _historyLogService.PrepareForInsert($"نمایش اطلاعات گزارش فیلتر لاگ برای فرم {data.TableName}", EnumFormName.ActivityLogFilters, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "ActivityLogFilters_Edit", type: EnumOperation.Get, table: EnumFormName.ActivityLogFilters, section: "نمایش ایجاد گزارش فعال")]
    public async Task<IActionResult> EditSubmit(ActivityLogFiltersDto command)
    {
        var res = await _activityLogFiltersService.Update(command);
        _historyLogService.PrepareForInsert(
            res.Success
                ? $"عملیات ویرایش فیلتر لاگ ها برای فرم {command.TableName} با موفقیت انجام شد."
                : $"عملیات ویرایش فیلتر لاگ ها برای فرم {command.TableName} با خطا مواجه شد.",
            EnumFormName.ActivityLogFilters,
            EnumOperation.Update);

        if (res.Success)
            TempData["SuccessMessage"] = $"عملیات ویرایش فیلتر لاگ ها برای فرم {command.TableName} با موفقیت انجام شد.";

        return new JsonResult(res);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "ActivityLogFilters_Delete", type: EnumOperation.Delete, table: EnumFormName.RoleRestriction, section: "حذف مسدود کردن نقش")]
    public async Task<IActionResult> Delete(long id)
    {
        var res = await _activityLogFiltersService.Delete(id);

        _historyLogService.PrepareForInsert(
            res.Success
                ? $"عملیات حذف لاگ ها برای فرم {id} با موفقیت انجام شد."
                : $"عملیات حذف لاگ ها برای فرم {id} با خطا مواجه شد.",
            EnumFormName.ActivityLogFilters,
            EnumOperation.Update);

        if (res.Success)
            TempData["SuccessMessage"] = $"عملیات حذف لاگ ها با آیدی {id} با موفقیت انجام شد.";

        return new JsonResult(res);
    }
}

