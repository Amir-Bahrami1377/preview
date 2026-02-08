using FormerUrban_Afta.Attributes;
using FormerUrban_Afta.DataAccess.DTOs.Marahel;
using FormerUrban_Afta.DataAccess.Model;

namespace FormerUrban_Afta.Areas.Marahel.Controllers;
[Area("Marahel")]
public class TaeedErsalController : AmardBaseController
{
    private readonly IDarkhastService _darkhastService;
    private readonly IParvandehService _parvandehService;
    private readonly IHistoryLogService _historyLogService;
    private readonly IEventLogThresholdService _eventLogThresholdService;
    private readonly TaeedErsalService _taeedErsalService;
    private readonly IValidator<TaeedErsalDto> _validator;
    public TaeedErsalController(IDarkhastService darkhastService, IParvandehService parvandehService,
        IHistoryLogService historyLogService, IEventLogThresholdService eventLogThresholdService
        , TaeedErsalService taeedErsalService, IValidator<TaeedErsalDto> validator)
    {
        _darkhastService = darkhastService;
        _parvandehService = parvandehService;
        _historyLogService = historyLogService;
        _eventLogThresholdService = eventLogThresholdService;
        _taeedErsalService = taeedErsalService;
        _validator = validator;
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "TaeedErsal", type: EnumOperation.Get, table: EnumFormName.Erja, section: "مشاهده تایید و ارسال")]
    public async Task<IActionResult> Index(int codeMarhaleh, int shod)
    {
        var darkhast = await _darkhastService.GetDataByShod(shod);
        var marahelList = Enum.GetValues(typeof(EnumMarhalehType))
            .Cast<EnumMarhalehType>()
            .Select(e => new EnumMarhalehTypeInfo
            {
                Name = e.ToString(),
                Index = (int)e,
                DisplayName = e.GetType()
                    .GetMember(e.ToString())[0]
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            })
            .ToList();

        var marhale = marahelList.FirstOrDefault(c => c.Index == codeMarhaleh)?.DisplayName;
        var marahel = marahelList.Where(c => c.Index > codeMarhaleh).ToList();

        var model = new TaeedErsalDto
        {
            shop = darkhast.shop,
            shod = shod,
            marhale = marhale,
            MarahelMandeh = marahel
        };

        _historyLogService.PrepareForInsert($"مشاهده تایید و ارسال درخواست {shod}", EnumFormName.Erja,EnumOperation.Get);

        return PartialView(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [CheckUserAccess(permissionCode: "TaeedErsal", type: EnumOperation.Post, table: EnumFormName.Erja, section: "تایید و ارسال به مرحله بعد")]
    public async Task<IActionResult> SabtErsal(TaeedErsalDto command)
    {
        var result = _validator.Validate(command);
        if (!result.IsValid)
        {
            _historyLogService.PrepareForInsert($"خطای اعتبارسنجی داده ها در شماره درخواست {command.shod}", EnumFormName.Erja, EnumOperation.Post);
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            return new JsonResult(new { success = false, message = errorMessages });
        }

        var marahelList = Enum.GetValues(typeof(EnumMarhalehType))
            .Cast<EnumMarhalehType>()
            .Select(e => new EnumMarhalehTypeInfo
            {
                Name = e.ToString(),
                Index = (int)e,
                DisplayName = e.GetType()
                    .GetMember(e.ToString())[0]
                    .GetCustomAttribute<DisplayAttribute>()?.Name ?? e.ToString()
            })
            .ToList();

        var oMarhalehJadid = marahelList.Where(c => c.Index == command.codeMarhale).FirstOrDefault();

        var res = await _taeedErsalService.SendToNextMarhaleh(oMarhalehJadid, command.shod);
        if (res)
        {
            _historyLogService.PrepareForInsert($"ارسال درخواست {command.shod} به مرحله بعد با موفقیت انجام شد.", EnumFormName.Erja, EnumOperation.Post);
            TempData["SuccessMessage"] = $"درخواست شماره {command.shod} با موفقیت به مرحله {oMarhalehJadid.DisplayName} ارسال شد.";
            return new JsonResult(new { success = true, message = $"درخواست {command.shod} با موفقیت به مرحله {oMarhalehJadid.DisplayName} ارجاع شد" });
        }
        _historyLogService.PrepareForInsert($"ارسال درخواست {command.shod} به مرحله بعد با خطا مواجه شده است.", EnumFormName.Erja, EnumOperation.Post);
        return new JsonResult(new { succes = false, message = "خطایی رخ داده است" });
    }
}
