using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;

namespace FormerUrban_Afta.Areas.Marahel.Controllers;

[Area("Marahel")]
public class ParvanehController : AmardBaseController
{
    private readonly IParvanehService _parvanehService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly IValidator<ParvanehDto> _validator;
    private readonly ILogger<ParvanehController> _logger;

    public ParvanehController(IParvanehService parvanehService, IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService, ILogger<ParvanehController> logger, IValidator<ParvanehDto> validator)
    {
        _parvanehService = parvanehService;
        _historyLogService = historyLogService;
        this._eventLogThresholdService = eventLogThresholdService;
        _eventLogThresholdService = eventLogThresholdService;
        _logger = logger;
        _validator = validator;
    }

    [CheckUserAccess("Darkhast_Detaile", type: EnumOperation.Get, table: EnumFormName.Parvaneh, section: "مرحله صدور پروانه")]
    public async Task<IActionResult> Index(MarahelDto model)
    {
        ParvanehDto data;
        var exist = await _parvanehService.Exist(model.shod);

        if (exist)
            data = await _parvanehService.GetData(model.shod);
        else
        {
            data = await _parvanehService.AddFirstTime(model.shod);
            _historyLogService.PrepareForInsert($"ایجاد مرحله صدور پروانه برای شماره درخواست {model.shod} پرونده {model.shop}", EnumFormName.Parvaneh, EnumOperation.Get);
        }
        _historyLogService.PrepareForInsert($"مشاهده مرحله صدور پروانه درخواست {model.shod} پرونده {model.shop}", EnumFormName.Parvaneh, EnumOperation.Get);

        data.shop = model.shop;
        data.sh_darkhast = model.shod;
        data.codeMarhaleh = model.codeMarhaleh;

        return View(data);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess("Parvaneh_Edit", type: EnumOperation.Update, table: EnumFormName.Parvaneh, section: "ویرایش مرحله صدور پروانه")]
    public async Task<IActionResult> Update(ParvanehDto parvanehDto)
    {
        var result = await _validator.ValidateAsync(parvanehDto);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطا اعتبار سنجی در ویرایش پروانه درخواست {parvanehDto.sh_darkhast}", EnumFormName.Parvaneh, EnumOperation.Update);
            parvanehDto.message = result.Errors.Select(e => e.ErrorMessage).ToList();
            return View("Index", parvanehDto);
        }

        var res = await _parvanehService.UpdateModel(parvanehDto);
        if (!res)
        {
            _historyLogService.PrepareForInsert($"خطا در ویرایش مرحله صدور پروانه درخواست {parvanehDto.sh_darkhast}", EnumFormName.Parvaneh, EnumOperation.Update);
            parvanehDto.message = new List<string> { "خطایی رخ داده است لطفا با پشتیبانی تماس بگیرید" };
            return View("Index", parvanehDto);
        }

        _historyLogService.PrepareForInsert($" ویرایش مرحله صدور پروانه درخواست {parvanehDto.sh_darkhast}", EnumFormName.Parvaneh, EnumOperation.Update);
        TempData["SuccessMessage"] = $"ویرایش مرحله صدور پروانه درخواست {parvanehDto.sh_darkhast} با موفقیت انجام شد!";
        return RedirectToAction("Index", new { shop = parvanehDto.shop, shod = parvanehDto.sh_darkhast, codeMarhaleh = parvanehDto.codeMarhaleh });
    }
}
