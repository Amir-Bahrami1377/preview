using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;
using FormerUrban_Afta.DataAccess.Model;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace FormerUrban_Afta.Areas.Parvandeh.Controllers;

[Area("Parvandeh")]
public class MalekinController : AmardBaseController
{
    private readonly IDv_malekinService _malekinService;
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IValidator<Dv_malekinDTO> _validatorMalekin;
    private readonly MyFunctions _myFunctions;

    public MalekinController(IDv_malekinService malekinService, IEventLogThresholdService eventLogThresholdService,
        IHistoryLogService historyLogService, IValidator<Dv_malekinDTO> validatorMalekin, MyFunctions myFunctions)
    {
        _malekinService = malekinService;
        _eventLogThresholdService = eventLogThresholdService;
        _historyLogService = historyLogService;
        _validatorMalekin = validatorMalekin;
        _myFunctions = myFunctions;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Parvandeh_Malekin", type: EnumOperation.Get, table: EnumFormName.Dv_malekin, section: "مشاهده مالکین")]
    public async Task<IActionResult> Index(int shop, int shod, string NoeParvandeh, int dShop = 0, string message = "", int codeMarhaleh = 0)
    {
        ViewBag.shop = shop;
        ViewBag.shod = shod;
        ViewBag.NoeParvandeh = NoeParvandeh;
        ViewBag.message = message;
        ViewBag.dShop = dShop;
        ViewBag.codeMarhaleh = codeMarhaleh;
        ViewBag.radif = _myFunctions.GetRadif(shop, shod);
        var result = await _malekinService.GetMalekinForParvande(shop, shod);
        _historyLogService.PrepareForInsert($"نمایش مالکین پرونده {shop}", EnumFormName.Dv_malekin, EnumOperation.Get);
        return View(result);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Malekin_Create", type: EnumOperation.Get, table: EnumFormName.Dv_malekin, section: "مشاهده ایجاد مالک")]
    public IActionResult Create(long identity, int shop, string NoeParvandeh, int radif)
    {
        var data = new Dv_malekinDTO
        {
            mtable_name = NoeParvandeh,
            shop = shop,
            d_radif = radif,
        };

        _historyLogService.PrepareForInsert($"نمایش ایجاد مالکین پرونده {shop}", EnumFormName.Dv_malekin, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Malekin_Create", type: EnumOperation.Post, table: EnumFormName.Dv_malekin, section: "ایجاد مالک")]
    public async Task<IActionResult> CreateSubmit(Dv_malekinDTO model)
    {
        var result = _validatorMalekin.Validate(model);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا اعبار سنجی در ایجاد مالک پرونده {model.shop} ", EnumFormName.Dv_malekin, EnumOperation.Post);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }
        var res = await _malekinService.InsertByModel(model, model.mtable_name);
        if (!res)
        {
            _historyLogService.PrepareForInsert($"خطا در ایجاد مالک پرونده {model.shop} ", EnumFormName.Dv_malekin, EnumOperation.Post);
            return new JsonResult(new { success = false, message = "عملیات با خطا مواجه شده است." });
        }

        _historyLogService.PrepareForInsert($"عملیات ایجاد مالک برای پرونده {model.shop} با موفقیت انجام شد.", EnumFormName.Dv_malekin, EnumOperation.Post);
        TempData["SuccessMessage"] = $"مالک با نام {model.name + " " + model.family} برای پرونده {model.shop} با موفقیت ایجاد شد.";
        return new JsonResult(new { success = true, message = "عملیات با موفقیت انجام شد." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Malekin_Edit", type: EnumOperation.Get, table: EnumFormName.Dv_malekin, section: "مشاهده ویرایش مالک")]
    public async Task<IActionResult> Edit(long identity, int shop, string NoeParvandeh, int radif)
    {
        var data = await _malekinService.GetById(identity);
        data.mtable_name = NoeParvandeh;
        data.shop = shop;
        data.d_radif = radif;

        _historyLogService.PrepareForInsert($"نمایش ویرایش مالکین پرونده {shop}", EnumFormName.Dv_malekin, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Malekin_Edit", type: EnumOperation.Update, table: EnumFormName.Dv_malekin, section: "ویرایش مالک")]
    public async Task<IActionResult> EditSubmit(Dv_malekinDTO model)
    {
        ValidationResult result = _validatorMalekin.Validate(model);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا در اعبارسنجی ویرایش مالک {model.name + " " + model.family} پرونده {model.shop}", EnumFormName.Dv_malekin, EnumOperation.Update);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }

        var res = await _malekinService.Update(model);
        if (!res)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش مالک پرونده {model.shop} ", EnumFormName.Dv_malekin, EnumOperation.Update);
            return new JsonResult(new { success = false, message = "عملیات با خطا مواجه شده است." });
        }

        _historyLogService.PrepareForInsert($"عملیات ویرایش مالک برای پرونده {model.shop} با موفقیت انجام شد.", EnumFormName.Dv_malekin, EnumOperation.Update);
        TempData["SuccessMessage"] = $"مالک با نام {model.name + " " + model.family} برای پرونده {model.shop} با موفقیت ویرایش شد.";
        return new JsonResult(new { success = true, message = "عملیات با موفقیت انجام شد." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Malekin_Delete", type: EnumOperation.Delete, table: EnumFormName.Dv_malekin, section: "حذف مالک")]
    public async Task<IActionResult> Delete(long id, int shop, int shod, string noeParvandeh, int dShop)
    {
        var res = _malekinService.DeleteMalek(id, shop);

        var message = res ? "" : "عملیات با خطا مواجه شده است!";
        if(res)
            TempData["SuccessMessage"] = $"مالک شماره {id} برای پرونده {shop} با موفقیت حذف شد.";

        ViewBag.shop = shop;
        ViewBag.shod = shod;
        ViewBag.NoeParvandeh = noeParvandeh;
        ViewBag.message = message;
        ViewBag.dShop = dShop;
        ViewBag.radif = _myFunctions.GetRadif(shop, shod);
        var result = await _malekinService.GetMalekinForParvande(shop, shod);
        return View("Index",result);
    }
}
