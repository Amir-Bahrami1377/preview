using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Parvandeh;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace FormerUrban_Afta.Areas.Parvandeh.Controllers;
[Area("Parvandeh")]
public class ApartmanController : AmardBaseController
{
    private readonly IApartmanService _apartmanService;
    private readonly ILogger<ApartmanController> _logger;
    private readonly IValidator<ApartmanDto> _validatorApartman;
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly IHistoryLogService _historyLogService;

    public ApartmanController(IApartmanService apartmanService, ILogger<ApartmanController> logger, IValidator<ApartmanDto> validatorApartman, IEventLogThresholdService eventLogThresholdService, IHistoryLogService historyLogService)
    {
        _apartmanService = apartmanService;
        _logger = logger;
        _validatorApartman = validatorApartman;
        _eventLogThresholdService = eventLogThresholdService;
        _historyLogService = historyLogService;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Parvandeh_ApartemanDetail", type: EnumOperation.Get, table: EnumFormName.Apartman, section: "اطلاعات آپارتمان")]
    public async Task<IActionResult> Index(int strShoP, int shod, int dShop)
    {
        ApartmanDto apartman;
        if (shod > 0)
            apartman = await _apartmanService.GetRowByShop(strShoP, shod);
        else
            apartman = await _apartmanService.GetRowByShop(strShoP);


        _historyLogService.PrepareForInsert($"مشاهده اطلاعات آپارتمان درخواست {shod} پرونده {strShoP}", EnumFormName.Apartman, EnumOperation.Get);

        apartman.shod = shod;
        apartman.dShop = dShop;

        return PartialView(apartman);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "Aparteman_Edit", type: EnumOperation.Update, table: EnumFormName.Apartman, section: "ویرایش اطلاعات آپارتمان")]
    public async Task<IActionResult> UpdateApartman(ApartmanDto apartman, int shod, int dShop)
    {
        ValidationResult result = _validatorApartman.Validate(apartman);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ویرایش اطلاعات آپارتمان {apartman.shop}", EnumFormName.Apartman, EnumOperation.Update);
            apartman.message = result.Errors.Select(e => e.ErrorMessage).ToList();
            return View("Index", apartman);
        }

        apartman.sabti = apartman.tafkiki + "-" + apartman.fari + "-" + apartman.azFari + "-" + apartman.asli + "-" + apartman.bakhsh;
        await _apartmanService.UpdateByModel(apartman);
        _historyLogService.PrepareForInsert($"ویرایش اطلاعات آپارتمان پرونده {apartman.shop} با موفقیت انجام شد.", EnumFormName.Apartman, EnumOperation.Update);
        TempData["SuccessMessage"] = $"ویرایش پرونده آپارتمان به شماره {apartman.shop} با موفقیت انجام شد.";
        return View("Index", apartman);
    }
}