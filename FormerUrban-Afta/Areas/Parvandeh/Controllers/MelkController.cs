using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;
using FormerUrban_Afta.DataAccess.Model;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Parvandeh.Controllers;
[Area("Parvandeh")]
public class MelkController : AmardBaseController
{
    private readonly IMelkService _melkService;
    private readonly IValidator<MelkDto> _validatorMelk;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly MyFunctions _myFunctions;
    public MelkController(IMelkService melkService, IValidator<MelkDto> validatorMelk, IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService, MyFunctions myFunctions)
    {
        _melkService = melkService;
        _validatorMelk = validatorMelk;
        _historyLogService = historyLogService;
        _eventLogThresholdService = eventLogThresholdService;
        _myFunctions = myFunctions;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Parvandeh_MelkDetail", type: EnumOperation.Get, table: EnumFormName.Melk, section: "اطلاعات ملک")]
    public async Task<IActionResult> PropertyDetails(int shop, int shod = 0, int dShop = 0, int codeMarhaleh = 0)
    {
        var melk = shod > 0 ?
            await _melkService.GetData(shop, shod) :
            await _melkService.GetDataByShop(shop);

        if (shod > 0)
            melk.Active = false;

        melk.shop = shop;
        melk.sh_Darkhast = shod;
        ViewBag.dShop = dShop;
        ViewBag.codeMarhaleh = codeMarhaleh;
        melk.codeMarhaleh = codeMarhaleh;
        _historyLogService.PrepareForInsert(description: $"مشاهده اطلاعات ملک پرونده {shop}", formName: EnumFormName.Melk, operation: EnumOperation.Get);
        return PartialView(melk);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Melk_Edit", type: EnumOperation.Update, table: EnumFormName.Melk, section: "ویرایش اطلاعات ملک")]
    public async Task<IActionResult> UpdateMelk(MelkDto obj, int dShop)
    {
        ValidationResult result = _validatorMelk.Validate(obj);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ویرایش اطلاعات ملک {obj.shop}", EnumFormName.Melk, EnumOperation.Update);
            ViewBag.dShop = dShop;
            obj.message = result.Errors.Select(e => e.ErrorMessage).ToList();
            return View("PropertyDetails", obj);
        }

        await _melkService.Update(obj);
        ViewBag.dShop = dShop;
        _historyLogService.PrepareForInsert($"ویرایش اطلاعات ملک {obj.shop} با موفقیت انجام شد.", EnumFormName.Melk, EnumOperation.Update);
        TempData["SuccessMessage"] = $"ویرایش پرونده ملک به شماره {obj.shop} با موفقیت انجام شد.";
        return View("PropertyDetails", obj);
    }
}
