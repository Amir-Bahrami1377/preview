using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Parvandeh.Controllers;

[Area("Parvandeh")]
public class Dv_karbariController : AmardBaseController
{
    private readonly IDv_KarbariService _KarbariService;
    private readonly IValidator<Dv_karbariDTO> _validatorKarbari;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly ISabethaService _sabethaService;
    private readonly MyFunctions _myFunctions;
    public Dv_karbariController(IDv_KarbariService dv_KarbariService, IValidator<Dv_karbariDTO> validatorKarbari
        , IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService, ISabethaService sabethaService, MyFunctions myFunctions)
    {
        _KarbariService = dv_KarbariService;
        _validatorKarbari = validatorKarbari;
        _historyLogService = historyLogService;
        _eventLogThresholdService = eventLogThresholdService;
        _sabethaService = sabethaService;
        _myFunctions = myFunctions;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Parvandeh_Karbari", type: EnumOperation.Get, table: EnumFormName.Dv_karbari, section: "مشاهده کاربری ")]
    public IActionResult Index(int shop, int shod, string NoeParvandeh, int dShop, string message = "", string codeMarhale = "")
    {
        ViewBag.shop = shop;
        ViewBag.shod = shod;
        ViewBag.dShop = dShop;


        var radif = _myFunctions.GetRadif(shop, shod);
        ViewBag.radif = radif;
        ViewBag.NoeParvandeh = NoeParvandeh != null ? NoeParvandeh : _myFunctions.GetStrNoeParvandeh(shop);
        ViewBag.codeMarhale = codeMarhale;
        ViewBag.parvandehOrBazdid = "";

        if (!string.IsNullOrWhiteSpace(codeMarhale))
            if(Convert.ToInt32(codeMarhale) > 1)
                ViewBag.parvandehOrBazdid = codeMarhale;
        
        var controller = codeMarhale switch
        {
            "2" => "ControlMap",
            "3" => "Estelam",
            "4" => "Parvaneh",
            _ => ""
        };
        var name = codeMarhale switch
        {
            "2" => "کنترل نقشه",
            "3" => "پاسخ استعلام",
            "4" => "صدور پروانه",
            _ => ""
        };

        ViewBag.controller = controller;
        ViewBag.name = name;

        _historyLogService.PrepareForInsert(description: $"مشاهده کاربری های درخواست {shod} پرونده {shop}", formName: EnumFormName.Dv_karbari, operation: EnumOperation.Get);
        var model = _KarbariService.GetDataByRadif(shop, radif);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Karbari_Create", type: EnumOperation.Get, table: EnumFormName.Dv_karbari, section: "مشاهده ایجاد کاربری ")]
    public IActionResult Create(int shop, string NoeParvandeh, int radif, int codeMarhale)
    {
        var data = new Dv_karbariDTO
        {
            mtable_name = NoeParvandeh,
            shop = shop,
            CodeMarhale = codeMarhale,
            d_radif = radif,
            ItemTabagheh = _sabethaService.GetRows("EnumTabaghe"),
            ItemKarbari = _sabethaService.GetRows("EnumKarbariType"),
            ItemMarhaleh = _sabethaService.GetRows("EnumMarhalehSakht"),
            ItemNoeestefadeh = _sabethaService.GetRows("EnumNoeEstefadeh"),
            ItemNoesakhteman = _sabethaService.GetRows("EnumNoeSakhteman"),
            ItemNoesazeh = _sabethaService.GetRows("EnumNoeSaze"),
        };

        _historyLogService.PrepareForInsert($"نمایش ایجاد کاربری های پرونده {shop}", EnumFormName.Dv_karbari, EnumOperation.Get);
        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Karbari_Create", type: EnumOperation.Post, table: EnumFormName.Dv_karbari, section: "ایجاد کاربری")]
    public IActionResult CreateSubmit(Dv_karbariDTO dv_KarbariDTO)
    {
        var result = _validatorKarbari.Validate(dv_KarbariDTO);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا در اعتبارسنجی اطلاعات کاربری پرونده {dv_KarbariDTO.shop} ", EnumFormName.Dv_karbari, EnumOperation.Post);

            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }

        var res = _KarbariService.InsertByModel(dv_KarbariDTO, dv_KarbariDTO.mtable_name);
        if (!res)
        {
            _historyLogService.PrepareForInsert($"خطا در ایجاد اطلاعات کاربری پرونده {dv_KarbariDTO.shop} ", EnumFormName.Dv_karbari, EnumOperation.Post);
            return new JsonResult(new { succecs = false, message = "عملیات با خطا مواجه شده است." });
        }

        _historyLogService.PrepareForInsert($"عملیات ایجاد کاربری برای پرونده {dv_KarbariDTO.shop} با موفقیت انجام شد.", EnumFormName.Dv_karbari, EnumOperation.Post);
        TempData["SuccessMessage"] = $"ایجاد کاربری برای پرونده {dv_KarbariDTO.shop} با موفقیت انجام شد.";
        return new JsonResult(new { succecs = true, message = "" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Karbari_Edit", type: EnumOperation.Get, table: EnumFormName.Dv_karbari, section: "مشاهده ویرایش کاربری")]
    public IActionResult Edit(long identity, int shop, string NoeParvandeh, int radif, int codeMarhale)
    {
        var data = _KarbariService.GetById(identity);
        data.mtable_name = NoeParvandeh;
        data.shop = shop;
        data.CodeMarhale = codeMarhale;
        data.d_radif = radif;
        data.ItemTabagheh = _sabethaService.GetRows("EnumTabaghe");
        data.ItemKarbari = _sabethaService.GetRows("EnumKarbariType");
        data.ItemMarhaleh = _sabethaService.GetRows("EnumMarhalehSakht");
        data.ItemNoeestefadeh = _sabethaService.GetRows("EnumNoeEstefadeh");
        data.ItemNoesakhteman = _sabethaService.GetRows("EnumNoeSakhteman");
        data.ItemNoesazeh = _sabethaService.GetRows("EnumNoeSaze");

        _historyLogService.PrepareForInsert($"نمایش ویرایش اطلاعات کابری ها {shop}", EnumFormName.Dv_karbari, EnumOperation.Get);

        return PartialView(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Karbari_Edit", type: EnumOperation.Update, table: EnumFormName.Dv_karbari, section: "ویرایش کاربری")]
    public IActionResult EditSubmit(Dv_karbariDTO dv_KarbariDTO)
    {
        var result = _validatorKarbari.Validate(dv_KarbariDTO);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا در اعتبارسنجی ویرایش اطلاعات کاربری {dv_KarbariDTO.karbari} پرونده {dv_KarbariDTO.shop} ", EnumFormName.Dv_karbari, EnumOperation.Update);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }

        var res = _KarbariService.UpdateKarbari(dv_KarbariDTO);
        if (!res)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش اطلاعات کاربری {dv_KarbariDTO.karbari} پرونده {dv_KarbariDTO.shop} ", EnumFormName.Dv_karbari, EnumOperation.Update);
            return new JsonResult(new { succecs = false, message = "عملیات با خطا مواجه شده است." });
        }

        _historyLogService.PrepareForInsert($"عملیات ویرایش کاربری {dv_KarbariDTO.karbari} برای پرونده {dv_KarbariDTO.shop} با موفقیت انجام شد.", EnumFormName.Dv_karbari, EnumOperation.Update);
        TempData["SuccessMessage"] = $"ویرایش کاربری با شماره {dv_KarbariDTO.karbari} در پرونده {dv_KarbariDTO.shop} با موفقیت انجام شد.";
        return new JsonResult(new { succecs = true, message = "عملیات با موفقیت انجام شد." });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Karbari_Delete", type: EnumOperation.Delete, table: EnumFormName.Dv_karbari, section: "حذف کاربری")]
    public IActionResult DeleteKarbari(long id, int shop, int shod, int dShop, string NoeParvandeh, string codeMarhale)
    {
        var res = _KarbariService.DeleteById(id, shop);
        if (!res)
            _historyLogService.PrepareForInsert($"خطا در حذف اطلاعات کاربری درخواست {shod} پرونده {shop} ", EnumFormName.Dv_karbari, EnumOperation.Delete);
        _historyLogService.PrepareForInsert($"عملیات حذف کاربری درخواست {shod} برای پرونده {shop} با موفقیت انجام شد.", EnumFormName.Dv_karbari, EnumOperation.Delete);
        
        var message = res ? $"حذف کاربری درخواست {shod} در پرونده {shop} با موفقیت انجام شد." : "عملیات با خطا مواجه شده است!";

        ViewBag.shop = shop;
        ViewBag.shod = shod;
        ViewBag.dShop = dShop;

        if (!string.IsNullOrWhiteSpace(message))
            TempData["SuccessMessage"] = message;

        var radif = _myFunctions.GetRadif(shop, shod);
        ViewBag.radif = radif;
        ViewBag.NoeParvandeh = NoeParvandeh != null ? NoeParvandeh : _myFunctions.GetStrNoeParvandeh(shop);
        ViewBag.codeMarhale = codeMarhale;

        var controller = codeMarhale switch
        {
            "2" => "ControlMap",
            "3" => "Estelam",
            "4" => "Parvaneh",
            _ => ""
        };
        var name = codeMarhale switch
        {
            "2" => "کنترل نقشه",
            "3" => "پاسخ استعلام",
            "4" => "صدور پروانه",
            _ => ""
        };

        ViewBag.controller = controller;
        ViewBag.name = name;

        var model = _KarbariService.GetDataByRadif(shop, radif);
        return View("Index", model);
    }
}

