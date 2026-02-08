using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;
using ValidationResult = FluentValidation.Results.ValidationResult;

using FormerUrban_Afta.DataAccess.Model;
using FormerUrban_Afta.DataAccess.Services;
using FormerUrban_Afta.DataAccess.Services.Interfaces;

namespace FormerUrban_Afta.Areas.Parvandeh.Controllers;
[Area("Parvandeh")]
public class SakhtemanController : AmardBaseController
{
    private readonly ISakhtemanService _sakhtemanService;
    private readonly ILogger<SakhtemanController> _logger;
    private readonly IValidator<SakhtemanDto> _validatorSakhteman;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEventLogThresholdService _eventLogThresholdService;

    public SakhtemanController(ISakhtemanService sakhtemanService, ILogger<SakhtemanController> logger,
        IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService,
        IValidator<SakhtemanDto> sakhtemanValidator)
    {
        _sakhtemanService = sakhtemanService;
        _logger = logger;
        _historyLogService = historyLogService;
        _eventLogThresholdService = eventLogThresholdService;
        _validatorSakhteman = sakhtemanValidator;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode:"Parvandeh_SakhtemanDetail", type:EnumOperation.Get, table:EnumFormName.Sakhteman, section:"مشاهده اطلاعات ساختمان")]
    public IActionResult Index(int strShoP, int shod, int dShop = 0, int codeMarhaleh = 0)
    {
        var sakhteman = shod > 0 ? _sakhtemanService.GetDataByShop(strShoP, shod) : _sakhtemanService.GetDataByShop(strShoP);

        _historyLogService.PrepareForInsert($"مشاهده اطلاعات ساختمان شماره پرونده {strShoP}", EnumFormName.Sakhteman, EnumOperation.Get);

        sakhteman.shod = shod;
        sakhteman.dShop = dShop;
        ViewBag.codeMarhaleh = codeMarhaleh;

        return PartialView(sakhteman);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode:"Sakhteman_Edit", type:EnumOperation.Update, table: EnumFormName.Sakhteman, section:"ویرایش اطلاعات ساختمان")]
    public IActionResult UpdateSakhteman(SakhtemanDto obj)
    {
        ValidationResult result = _validatorSakhteman.Validate(obj);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ویرایش اطلاعات ساختمان {obj.shop}", EnumFormName.Sakhteman, EnumOperation.Update);
            obj.message = result.Errors.Select(e => e.ErrorMessage).ToList();
            return View("Index", obj);
        }

        _sakhtemanService.UpdateByModel(obj);
        _historyLogService.PrepareForInsert($"ویرایش اطلاعات ساختمان {obj.shop} با موفقیت انجام شد.", EnumFormName.Sakhteman, EnumOperation.Update);
        TempData["SuccessMessage"] = $"ویرایش پرونده ساختمان به شماره {obj.shop} با موفقیت انجام شد.";
        return View("Index", obj);
    }
}