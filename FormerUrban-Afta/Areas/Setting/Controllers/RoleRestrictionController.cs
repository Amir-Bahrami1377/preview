using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormerUrban_Afta.Areas.Setting.Controllers;

[Area("Setting")]
public class RoleRestrictionController : Controller
{
    private readonly IRoleRestrictionService _roleRestrictionService;
    private readonly IAuthService _authService;
    private readonly IValidator<RoleRestrictionDto> _validator;
    private readonly IHistoryLogService _historyLogService;

    public RoleRestrictionController(IRoleRestrictionService roleRestrictionService, IAuthService authService, IValidator<RoleRestrictionDto> validator, IHistoryLogService historyLogService)
    {
        _roleRestrictionService = roleRestrictionService;
        _authService = authService;
        _validator = validator;
        _historyLogService = historyLogService;
    }



    [CheckUserAccess(permissionCode: "Menu_RoleRestriction", type: EnumOperation.Get, table: EnumFormName.RoleRestriction, section: "نقش های مسدود شده")]
    public async Task<IActionResult> Index()
    {
        var model = await _roleRestrictionService.GetAll();
        _historyLogService.PrepareForInsert($"مشاهده اطلاعات نقش های مسدود شده", EnumFormName.RoleRestriction, EnumOperation.Get);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "RoleRestriction_Create", type: EnumOperation.Get, table: EnumFormName.RoleRestriction, section: "نمایش مسدود کردن نقش")]
    public async Task<IActionResult> Create()
    {
        var data = new RoleRestrictionDto
        {
            Roles = new SelectList(await _authService.GetAllRoleAsync(), "Id", "Description"),
        };
        _historyLogService.PrepareForInsert($"مشاهده اطلاعات ایجاد مسدود کردن نقش", EnumFormName.RoleRestriction, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "RoleRestriction_Create", type: EnumOperation.Post, table: EnumFormName.RoleRestriction, section: "مسدود کردن نقش")]
    public async Task<IActionResult> CreateSubmit(RoleRestrictionDto command)
    {
        var result = _validator.Validate(command);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطای اعتبار سنجی در مسدود سازی نقش {command.RoleText} ", EnumFormName.Darkhast, EnumOperation.Post);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }

        var res = await _roleRestrictionService.Add(command);

        if(res.Success)
            TempData["SuccessMessage"] = $"مسدود کردن نقش {command.RoleText} با موفقیت انجام شد.";

        _historyLogService.PrepareForInsert($"نقش {command.RoleText} با موفقیت مسدود شد.", EnumFormName.RoleRestriction, EnumOperation.Get);

        return new JsonResult(res);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "RoleRestriction_Edit", type: EnumOperation.Get, table: EnumFormName.RoleRestriction, section: "نمایش ویرایش مسدود کردن نقش")]
    public async Task<IActionResult> Edit(long identity)
    {
        var data = await _roleRestrictionService.GetById(id: identity);
        data.Roles = new SelectList(await _authService.GetAllRoleAsync(), "Id", "Description");
        _historyLogService.PrepareForInsert($"مشاهده ویرایش اطلاعات نقش مسدود شده {data.RoleText}", EnumFormName.RoleRestriction, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "RoleRestriction_Edit", type: EnumOperation.Update, table: EnumFormName.RoleRestriction, section: "ویرایش مسدود کردن نقش")]
    public async Task<IActionResult> EditSubmit(RoleRestrictionDto command)
    {
        var result = _validator.Validate(command);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطای اعتبار سنجی در مسدود سازی نقش با آیدی {command.Identity}", EnumFormName.Darkhast, EnumOperation.Post);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }

        var res = await _roleRestrictionService.Update(command);

        if(res.Success)
            TempData["SuccessMessage"] = $"مسدود کردن نقش {command.RoleText} با موفقیت انجام شد.";

        _historyLogService.PrepareForInsert($"ویرایش اطلاعات نقش مسدود شده با آیدی {command.RoleText}", EnumFormName.RoleRestriction, EnumOperation.Get);

        return new JsonResult(res);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "RoleRestriction_Delete", type: EnumOperation.Delete, table: EnumFormName.RoleRestriction, section: "حذف مسدود کردن نقش")]
    public async Task<IActionResult> Delete(long id)
    {
        var res = await _roleRestrictionService.Delete(id);

        if (res.Success)
            TempData["SuccessMessage"] = $"عملیات حذف نقش مسدود شده با آیدی {id} با موفقیت انجام شد.";
        _historyLogService.PrepareForInsert($"حذف نقش مسدود شده با آیدی {id}", EnumFormName.RoleRestriction, EnumOperation.Get);

        return new JsonResult(res);
    }

}
